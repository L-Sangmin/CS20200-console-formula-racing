module AI

open Types

let private removeAt (i: int) (xs: 'a list) : 'a * 'a list =
    List.item i xs,
    xs |> List.mapi (fun j x -> if j = i then None else Some x) |> List.choose id

let private checkFinish (team: TeamState) (state: GameState) : TeamState =
    if not team.Finished && team.Lap >= state.TargetLaps then Race.finishTeam team state
    else team

// ── card selection ────────────────────────────────────────────────────────────

let private pickCardIdx (rng: System.Random) (algo: AiAlgorithm) (cards: TireCard list) (weather: Weather) : int =
    match algo with
    | Greedy ->
        let best = cards |> List.maxBy (fun c -> max 1 (Movement.tireMove c.Tire weather + c.Variance))
        cards |> List.findIndex (fun c -> c = best)
    | RandomPlay | Precise ->
        rng.Next(List.length cards)

// ── tire selection for pit ────────────────────────────────────────────────────

let private pickTire (rng: System.Random) (algo: AiAlgorithm) (weather: Weather) : TireType =
    let tires = [| Soft; Medium; Hard |]
    match algo with
    | Greedy | Precise -> tires |> Array.maxBy (fun t -> Movement.tireMove t weather)
    | RandomPlay       -> tires.[rng.Next(3)]

// ── pit decision ──────────────────────────────────────────────────────────────

let private shouldPit (rng: System.Random) (algo: AiAlgorithm) (team: TeamState) (state: GameState) : bool =
    match algo with
    | Greedy -> List.length team.TireCards <= 1
    | RandomPlay -> rng.Next(2) = 0
    | Precise ->
        // Pit only when remaining cards can't cover remaining distance
        let cardsLeft = List.length team.TireCards
        if cardsLeft = 0 then true
        else
            let remaining = max 0 ((state.TargetLaps - team.Lap) * TrackLength - team.Position)
            let avgMove =
                match team.TireCards with
                | c :: _ -> Movement.tireMove c.Tire state.Weather
                | []     -> Movement.basicMove state.Weather
            remaining > cardsLeft * avgMove

// ── algo name for summary display ─────────────────────────────────────────────

let algoLabel (algo: AiAlgorithm) : string =
    match algo with
    | Greedy     -> "Greedy"
    | RandomPlay -> "Random"
    | Precise    -> "Precise"

// ── main AI turn (no print* calls; returns new state + summary string) ────────

let private runMoveTurn (rng: System.Random) (algo: AiAlgorithm) (team: TeamState) (state: GameState) : GameState * string =
    let cardOpt, team2 =
        match team.TireCards with
        | [] -> None, team
        | cards ->
            let idx  = pickCardIdx rng algo cards state.Weather
            let card, rest = removeAt idx cards
            Some card, { team with TireCards = rest }

    let move   = Movement.computeMove team2.Passive cardOpt state.Weather
    let state1 = { state with Teams = Race.updateTeam state.Teams team2 }

    if Movement.passesPitEntry team2.Position move && shouldPit rng algo team2 state1 then
        let tire   = pickTire rng algo state1.Weather
        let pitted = Pit.enterPit rng team2 tire
        let state2 = { state1 with Teams = Race.updateTeam state1.Teams pitted }
                     |> Race.advanceTurn
        state2, sprintf "entered pit → %s tires" (Render.tireLabel tire)
    else
        let moved, _ = Race.applyMovement team2 move
        let final    = checkFinish moved state1
        let state2   = { state1 with Teams = Race.updateTeam state1.Teams final }
                       |> Race.advanceTurn
        let lapStr   = sprintf "Lap %d/%d" final.Lap state1.TargetLaps
        let fin      = if final.Finished then " ★FINISHED" else ""
        state2, sprintf "pos %d (%s)%s" final.Position lapStr fin

let runTurn (rng: System.Random) (team: TeamState) (state: GameState) : GameState * string =
    if team.InPit then
        let exited = Pit.exitPit team
        let state1 = { state with Teams = Race.updateTeam state.Teams exited }
        let final  = checkFinish exited state1
        if final.Finished then
            let state2 = { state1 with Teams = Race.updateTeam state1.Teams final }
                         |> Race.advanceTurn
            state2, sprintf "exited pit (Lap %d/%d) ★FINISHED" final.Lap state1.TargetLaps
        else
            let state2, moveSummary = runMoveTurn rng team.Algorithm final state1
            state2, sprintf "exited pit → %s" moveSummary
    else
        runMoveTurn rng team.Algorithm team state
