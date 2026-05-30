module Render

open Types

// ── label helpers ──────────────────────────────────────────────────────────

let weatherLabel (w: Weather) : string =
    match w with
    | Sunny -> "Sunny"
    | Rainy -> "Rainy"

let tireLabel (t: TireType) : string =
    match t with
    | Soft   -> "Soft"
    | Medium -> "Medium"
    | Hard   -> "Hard"

let passiveLabel (p: PassiveAbility) : string =
    match p with
    | WarmTires    -> "Warm Tires    (+2 Soft move, any weather)"
    | RainEngineer -> "Rain Engineer (+2 move in Rainy)"
    | PitCrew      -> "Pit Crew      (+2 cards on pit replenish)"
    | Endurance    -> "Endurance     (+2 basic move)"

let passiveShortLabel (p: PassiveAbility) : string =
    match p with
    | WarmTires    -> "Warm Tires"
    | RainEngineer -> "Rain Engineer"
    | PitCrew      -> "Pit Crew"
    | Endurance    -> "Endurance"

let private teamNameTag (t: TeamState) : string =
    match t.Kind with
    | AI    -> sprintf "%s[CPU]" t.Name
    | Human -> t.Name

let kindLabel (k: TeamKind) : string =
    match k with
    | Human -> "YOU"
    | AI    -> "CPU"

// Team letter for track: Id 1 -> "A", Id 2 -> "B", ...
let teamLetter (id: int) : string =
    string (char (id + int 'A' - 1))

// ── track rendering ─────────────────────────────────────────────────────────
// Circuit layout (U-shaped oval, two 10-cell rows):
//   Top row  (→): positions  0–9    (S/F line at pos 0, left side)
//   Pit lane:     off-track branch  (entry at pos 18, exit rejoins at pos 0)
//   Bottom row(←): positions 19–10 (pos 18 = P.E, displayed right-to-left)
//
// Pit mechanics: passing pos 18 → pit option offered
//   enter: team moves off-track (Position = 20)
//   exit : team placed at pos 0 (S/F crossing = +1 lap), then moves normally
//
// Each cell: 8 chars  ┌──NN──┐ / │CCCCCC│ / └──────┘  (5 rows: top + blank + content + blank + bottom)

// Build one main-track cell: 5-element list, each string 8 chars
// Teams in the pit (Position = PitSpacePos = 20) are NOT shown here
let private buildBoxCell (state: GameState) (pos: int) : string list =
    let top  = sprintf "┌──%02d──┐" pos
    let bot  = "└──────┘"
    let here =
        state.Teams
        |> List.filter (fun t -> t.Position = pos && not t.Finished && not t.InPit)
    let content =
        match here with
        | [] -> " .... "
        | teams ->
            let letters = teams |> List.truncate 4 |> List.map (fun t -> teamLetter t.Id) |> String.concat ""
            (" " + letters).PadRight(6)
    [ top; "│      │"; sprintf "│%s│" content; "│      │"; bot ]

// ── pit team list ────────────────────────────────────────────────────────────

let private cardInfo (t: TeamState) : string =
    match t.TireCards with
    | []     -> "no cards"
    | c :: _ -> sprintf "%s x%d" (tireLabel c.Tire) (List.length t.TireCards)

// Teams currently in the pit lane, shown as a labelled list below the circuit
let private renderPitTeams (state: GameState) : string =
    let pitTeams = state.Teams |> List.filter (fun t -> t.InPit && not t.Finished)
    match pitTeams with
    | [] -> ""
    | ts ->
        let rows =
            ts |> List.map (fun t ->
                sprintf "  %s  %-10s [%s]  %s"
                    (teamLetter t.Id) t.Name (kindLabel t.Kind) (cardInfo t))
        "=== Teams in Pit ===\n" + String.concat "\n" rows

// Full track: left/right U-turn connectors + S/F marker + pit entry indicator + pit teams
// 8-char cells: 10×8=80 + 5-char left prefix + 4-char right connector = 89 per line
// Cells have 5 lines: [top, blank, content, blank, bottom]
// Pit entry indicator aligns under pos 18 col 16 (5 prefix + 8 for pos19 + 3 into pos18)
let renderTrack (state: GameState) : string =
    let topCells = [ 0..9 ]       |> List.map (buildBoxCell state)
    let botCells = [ 19..-1..10 ] |> List.map (buildBoxCell state)
    let raw cells i = cells |> List.map (List.item i) |> String.concat ""
    let t0 = raw topCells 0   // top borders
    let t1 = raw topCells 1   // blank top
    let t2 = raw topCells 2   // content
    let t3 = raw topCells 3   // blank bottom
    let t4 = raw topCells 4   // bottom borders
    let b0 = raw botCells 0
    let b1 = raw botCells 1
    let b2 = raw botCells 2
    let b3 = raw botCells 3
    let b4 = raw botCells 4
    let circuit =
        [ "        │ pit exit (+1 lap)"
          "        ▼"    // ▼ at col 8, directly above pos 00
          sprintf "     %s    " t0          // top borders
          sprintf "     %s    " t1          // blank top
          sprintf " ┌─▶ %s ──┐" t2         // content: left entry arrow + right turn corner
          sprintf " │   %s   │" t3   // blank bottom, right │
          sprintf " │   %s   │" t4   // bottom borders, right │
          " │                                                                                      │"
          "─┼─ Start/Finish (+1 lap if you cross it)                                               │"
          " │                                                                                      │"
          sprintf " │   %s   │" b0   // top borders, right │
          sprintf " │   %s   │" b1   // blank top, right │
          sprintf " └── %s ◀─┘" b2   // content: left exit + right turn corner
          sprintf "     %s    " b3   // blank bottom, no connector
          sprintf "     %s    " b4   // bottom borders, no connector
          "                │"
          "                ▼  pit entry" ]
    let pitTeams = renderPitTeams state
    let parts =
        [ String.concat "\n" circuit ]
        @ (if pitTeams = "" then [] else [ ""; pitTeams ])
    String.concat "\n" parts

// ── standings ──────────────────────────────────────────────────────────────

let private teamStatus (t: TeamState) (targetLaps: int) : string =
    if t.Finished then
        sprintf "FINISHED  (rank #%d)" (t.FinishRank |> Option.defaultValue 0)
    elif t.InPit then
        sprintf "Lap %d/%d  IN PIT  (%s)" t.Lap targetLaps (cardInfo t)
    else
        sprintf "Lap %d/%d  Pos %2d  %s" t.Lap targetLaps t.Position (cardInfo t)

let renderStandings (state: GameState) : string =
    let rows =
        Race.computeStandings state
        |> List.map (fun (t, rank) ->
            sprintf "  %d. %-29s %s"
                rank (teamNameTag t) (teamStatus t state.TargetLaps))
    "STANDINGS:\n" + (rows |> String.concat "\n")

// ── hand display ───────────────────────────────────────────────────────────

let private tireTypeOrder (t: TireType) : int =
    match t with Soft -> 0 | Medium -> 1 | Hard -> 2

// Sort by type (Soft→Medium→Hard) then by move value descending within type
let sortCards (cards: TireCard list) (weather: Weather) : TireCard list =
    cards |> List.sortWith (fun a b ->
        let ta = tireTypeOrder a.Tire
        let tb = tireTypeOrder b.Tire
        if ta <> tb then compare ta tb
        else compare (Movement.cardMove b weather) (Movement.cardMove a weather))

let renderHand (team: TeamState) (weather: Weather) : string =
    let passiveDesc =
        match team.Passive with
        | WarmTires    -> "+2 for Soft tires"
        | RainEngineer -> sprintf "+2 in Rainy%s" (if weather = Rainy then " (active)" else " (inactive)")
        | Endurance    -> "+2 on basic move"
        | PitCrew      -> "+2 cards on pit stop"
    let header = sprintf "YOUR HAND [%s]" passiveDesc
    match team.TireCards with
    | [] ->
        let mv = Movement.computeMove team.Passive None weather
        sprintf "%s: (empty) — basic move: %d in %s" header mv (weatherLabel weather)
    | cards ->
        let sorted = sortCards cards weather
        let cardStrs =
            sorted |> List.map (fun card ->
                let base_ = Movement.cardMove card weather
                let total  = Movement.applyPassive team.Passive (Some card.Tire) weather base_
                let bonus  = total - base_
                if bonus > 0 then sprintf "%s[%d] +%d" (tireLabel card.Tire) base_ bonus
                else            sprintf "%s[%d]"       (tireLabel card.Tire) base_)
        header + "\n  " + String.concat " | " cardStrs

// ── full state display ─────────────────────────────────────────────────────

let sep = String.replicate 90 "-"

let renderAll (state: GameState) (aiLog: string list) : unit =
    let currentId =
        if state.TurnIndex < List.length state.TurnOrder
        then List.item state.TurnIndex state.TurnOrder
        else -1
    let currentTeam = state.Teams |> List.tryFind (fun t -> t.Id = currentId)
    let turnLabel =
        match currentTeam with
        | Some t -> sprintf "%s (%s)" t.Name (kindLabel t.Kind)
        | None   -> "—"

    printfn ""
    printfn "%s" sep
    printfn " Round %-2d | Weather: %-5s | Turn: %s"
        state.CurrentRound (weatherLabel state.Weather) turnLabel
    printfn "%s" sep
    if not (List.isEmpty aiLog) then
        printfn " AI turns:"
        aiLog |> List.iter (fun msg -> printfn "  %s" msg)
        printfn "%s" sep
    printfn " TRACK:"
    printfn "%s" (renderTrack state)
    printfn "%s" sep
    printfn "%s" (renderStandings state)
    printfn "%s" sep
    let humanTeam = state.Teams |> List.tryFind (fun t -> t.Kind = Human)
    match humanTeam with
    | Some t when not t.Finished ->
        printfn " %s" (renderHand t state.Weather)
        printfn "%s" sep
    | _ -> ()

// ── event messages ─────────────────────────────────────────────────────────

let printMoved (team: TeamState) (lapsGained: int) : unit =
    if lapsGained > 0 then
        printfn " >> %s crossed the start/finish line! (now Lap %d/%d)"
            team.Name team.Lap TargetLaps
    else
        printfn " >> %s moved to position %d  (Lap %d/%d)"
            team.Name team.Position team.Lap TargetLaps

let printBasicMove (team: TeamState) : unit =
    printfn " >> %s has no tire cards — using basic movement." team.Name

let printPitOption (team: TeamState) : unit =
    printfn " >> %s's move passes the pit entry." team.Name

let printPitEntered (team: TeamState) : unit =
    printfn " >> %s entered the pit!" team.Name

let printPitExited (team: TeamState) : unit =
    printfn " >> %s left the pit." team.Name

let printPitReplenished (team: TeamState) (tire: TireType) (count: int) : unit =
    printfn " >> %s chose %s tires and received %d card(s)."
        team.Name (tireLabel tire) count

let printWeatherChange (weather: Weather) (round: int) : unit =
    printfn ""
    printfn " *** Round %d begins — Weather: %s ***" round (weatherLabel weather)

let printFinished (team: TeamState) : unit =
    let rank = team.FinishRank |> Option.defaultValue 0
    let rankStr =
        match rank with
        | 1 -> "1st"
        | 2 -> "2nd"
        | 3 -> "3rd"
        | n -> sprintf "%dth" n
    printfn ""
    printfn " *** %s finished in %s place! ***" team.Name rankStr

let printFinalResults (state: GameState) : unit =
    printfn ""
    printfn "%s" sep
    printfn " FINAL RESULTS"
    printfn "%s" sep
    state.Teams
    |> List.sortBy (fun t -> t.FinishRank |> Option.defaultValue 999)
    |> List.iter (fun t ->
        let rank = t.FinishRank |> Option.defaultValue 0
        printfn "  %d. %s" rank t.Name)
    printfn "%s" sep
