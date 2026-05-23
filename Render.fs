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
// Each cell: 6 chars  ┌─NN─┐ / │CCCC│ / └────┘  (3 rows, no lap-count row)

// Space-type label embedded in cell content (4 chars)
let private spChar (pos: int) : string =
    if   pos = 0           then "S"   // Start / Finish
    elif pos = PitEntryPos then "!"   // Pit Entry (pos 18)
    else                        "."   // Normal

// Build one main-track cell: [topBorder; content; bottomBorder], each 6 chars
// Teams in the pit (Position = PitSpacePos = 20) are NOT shown here
let private buildBoxCell (state: GameState) (pos: int) : string list =
    let top  = sprintf "┌─%02d─┐" pos
    let bot  = "└────┘"
    let here =
        state.Teams
        |> List.filter (fun t -> t.Position = pos && not t.Finished && not t.InPit)
    let sp = spChar pos
    let row =
        match here with
        | [] ->
            match pos with
            | 0                      -> "│S/F │"
            | p when p = PitEntryPos -> "│P.E │"
            | _                      -> "│ .. │"
        | [t]    -> sprintf "│ %s%s │" (teamLetter t.Id) sp
        | t :: _ -> sprintf "│ %s+%s│" (teamLetter t.Id) sp
    [ top; row; bot ]

// Concatenate one line-index across all cells, prefixed with one space
let private segLine (cells: string list list) (i: int) : string =
    " " + (cells |> List.map (List.item i) |> String.concat "")

// 3-line segment with direction arrow on the top-border line
let private buildSegment (state: GameState) (positions: int list) (arrow: string) : string =
    let cells = positions |> List.map (buildBoxCell state)
    sprintf "%s %s\n%s\n%s"
        (segLine cells 0) arrow
        (segLine cells 1)
        (segLine cells 2)

// ── pit connectors and team list ─────────────────────────────────────────────
// Column alignment (0-indexed from line start):
//   pos 00 in top row    → col 3  (prefix " " + "┌─" = 3 chars before "00")
//   pos 18 in bottom row → col 9  (prefix " " + "┌─19─┐┌─" = 9 chars before "18")
//
// Pit exit: a line/arrow above the top row pointing DOWN into pos 00
// Pit entry: a line/arrow below the bottom row pointing DOWN from pos 18

// Three lines above the top row: corner turn + vertical drop + arrowhead into pos 00
let private pitExitHeader : string =
    "───┐ pit exit (+1 lap)\n   │\n   ▼"

// Three lines below the bottom row: vertical drop + arrowhead from pos 18 into pit
let private pitEntryFooter : string =
    "         │\n         │\n         ▼  pit entry (pos 18)"

let private cardInfo (t: TeamState) : string =
    match t.TireCards with
    | []     -> "no cards"
    | c :: _ -> sprintf "%s x%d" (tireLabel c) (List.length t.TireCards)

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

// Full track: pit-exit header / top segment / gap / bottom segment / pit-entry footer / pit teams
let renderTrack (state: GameState) : string =
    let top      = buildSegment state [ 0 .. 9 ]         "→"
    let bot      = buildSegment state [ 19 .. -1 .. 10 ] "←"
    let pitTeams = renderPitTeams state
    let parts =
        [ pitExitHeader; top; ""; bot; pitEntryFooter ]
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
            sprintf "  %d. %-10s [%s]  %s"
                rank t.Name (kindLabel t.Kind) (teamStatus t state.TargetLaps))
    "STANDINGS:\n" + (rows |> String.concat "\n")

// ── hand display ───────────────────────────────────────────────────────────

let renderHand (team: TeamState) (weather: Weather) : string =
    match team.TireCards with
    | [] ->
        let mv = Movement.basicMove weather
        sprintf "HAND: no tire cards — basic move: %d space(s) in %s" mv (weatherLabel weather)
    | cards ->
        let ttype = List.head cards
        let count = List.length cards
        let mv    = Movement.tireMove ttype weather
        sprintf "HAND: %s x%d — one card moves %d space(s) in %s"
            (tireLabel ttype) count mv (weatherLabel weather)

// ── full state display ─────────────────────────────────────────────────────

let sep = String.replicate 63 "-"

let renderAll (state: GameState) : unit =
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
    printfn " TRACK:"
    printfn "%s" (renderTrack state)
    printfn "%s" sep
    printfn "%s" (renderStandings state)
    printfn "%s" sep
    // Show hand only on the human player's turn
    match currentTeam with
    | Some t when t.Kind = Human && not t.Finished ->
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
