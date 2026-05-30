module Program

open Types

let private rng = System.Random()

let private rollWeather () : Weather =
    if rng.Next(2) = 0 then Sunny else Rainy

let private removeAt (i: int) (xs: 'a list) : 'a * 'a list =
    List.item i xs,
    xs |> List.mapi (fun j x -> if j = i then None else Some x) |> List.choose id

let private checkFinish (team: TeamState) (state: GameState) : TeamState =
    if not team.Finished && team.Lap >= state.TargetLaps then
        let t = Race.finishTeam team state
        Render.printFinished t
        t
    else team

let private moveAndCheck (team: TeamState) (cardOpt: TireCard option) (state: GameState) : TeamState =
    let move = Movement.computeMove team.Passive cardOpt state.Weather
    let moved, lapsGained = Race.applyMovement team move
    Render.printMoved moved lapsGained
    checkFinish moved state

// ── human card & tire selection ───────────────────────────────────────────────

let private humanPickCard (team: TeamState) (state: GameState) : TireCard option * TeamState =
    match team.TireCards with
    | [] ->
        Render.printBasicMove team
        None, team
    | cards ->
        let sorted = Render.sortCards cards state.Weather
        printfn ""
        printfn " Pick a card to play: (Weather: %s)" (Render.weatherLabel state.Weather)
        sorted
        |> List.mapi (fun i card ->
            let base_ = Movement.cardMove card state.Weather
            let total  = Movement.applyPassive team.Passive (Some card.Tire) state.Weather base_
            let bonus  = total - base_
            let bonusStr = if bonus > 0 then sprintf " +%d" bonus else ""
            let cardStr = sprintf "%s[%d]%s" (Render.tireLabel card.Tire) base_ bonusStr
            sprintf "%2d. %-14s" (i + 1) cardStr)
        |> List.chunkBySize 5
        |> List.iter (fun chunk -> printfn "  %s" (String.concat "" chunk))
        let idx = Input.promptInt " Card" 1 (List.length sorted)
        let card, rest = removeAt (idx - 1) sorted
        Some card, { team with TireCards = rest }

let private humanChooseTire (team: TeamState) (weather: Weather) : TireType =
    let tires = [ Soft; Medium; Hard ]
    printfn ""
    printfn " Choose tire type:"
    tires |> List.iteri (fun i t ->
        let count = Pit.tireCardCount t
        let bonus = match team.Passive with PitCrew -> sprintf " (+2 → %d)" (count + 2) | _ -> ""
        printfn "  %d. %-6s  Sunny:%d  Rainy:%d  Cards:%d%s"
            (i + 1) (Render.tireLabel t)
            (Movement.tireMove t Sunny) (Movement.tireMove t Rainy)
            count bonus)
    let idx = Input.promptInt " Tire" 1 3
    tires.[idx - 1]

// ── human turn execution ──────────────────────────────────────────────────────

let private humanTakeTurn (team: TeamState) (state: GameState) : GameState =
    let cardOpt, team2 = humanPickCard team state
    let move   = Movement.computeMove team2.Passive cardOpt state.Weather
    let state1 = { state with Teams = Race.updateTeam state.Teams team2 }

    if Movement.passesPitEntry team2.Position move then
        Render.printPitOption team2
        printf " Enter pit? [y/n]: "
        let ans = (Input.readLine ()).Trim().ToLower()
        if ans = "y" || ans = "yes" then
            let tire   = humanChooseTire team2 state1.Weather
            let pitted = Pit.enterPit rng team2 tire
            Render.printPitEntered pitted
            Render.printPitReplenished pitted tire (List.length pitted.TireCards)
            { state1 with Teams = Race.updateTeam state1.Teams pitted }
            |> Race.advanceTurn
        else
            let moved = moveAndCheck team2 cardOpt state1
            { state1 with Teams = Race.updateTeam state1.Teams moved }
            |> Race.advanceTurn
    else
        let moved = moveAndCheck team2 cardOpt state1
        { state1 with Teams = Race.updateTeam state1.Teams moved }
        |> Race.advanceTurn

let private runHumanTurn (team: TeamState) (state: GameState) : GameState =
    if team.InPit then
        let exited = Pit.exitPit team
        Render.printPitExited exited
        let state1 = { state with Teams = Race.updateTeam state.Teams exited }
        let final  = checkFinish exited state1
        if final.Finished then
            { state1 with Teams = Race.updateTeam state1.Teams final }
            |> Race.advanceTurn
        else
            humanTakeTurn final state1
    else
        humanTakeTurn team state

// ── main race loop ────────────────────────────────────────────────────────────

let rec private runRace (state: GameState) (aiLog: string list) : GameState =
    if Race.raceOver state then
        Render.printFinalResults state
        state
    else
        let state1 =
            if Race.isNewRound state then
                let w = rollWeather ()
                Render.printWeatherChange w state.CurrentRound
                { state with Weather = w }
            else state

        let currentId = List.item state1.TurnIndex state1.TurnOrder
        let current   = state1.Teams |> List.find (fun t -> t.Id = currentId)

        if current.Finished then
            runRace (Race.advanceTurn state1) aiLog
        elif current.Kind = AI then
            printfn ""
            printf " · %s's turn..." current.Name
            let state2, summary = AI.runTurn rng current state1
            printfn " done"
            let entry = sprintf "%-10s [%s|%s]: %s" current.Name (AI.algoLabel current.Algorithm) (Render.passiveShortLabel current.Passive) summary
            runRace state2 (aiLog @ [entry])
        else
            Render.renderAll state1 aiLog
            let state2 = runHumanTurn current state1
            runRace state2 []

[<EntryPoint>]
let main _ =
    try
        let state = Setup.setupGame ()
        runRace state [] |> ignore
        0
    with Input.QuitGame ->
        printfn "\n Game quit."
        0
