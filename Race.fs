module Race

open Types

// Apply movement; returns (updated team, laps gained this move) (Req 18)
// Handles lap wrap: rawPos >= TrackLength means finish line was crossed
let applyMovement (team: TeamState) (move: int) : TeamState * int =
    let rawPos    = team.Position + move
    let lapsGained = rawPos / TrackLength
    let newPos    = rawPos % TrackLength
    { team with Position = newPos; Lap = team.Lap + lapsGained }, lapsGained

// Pop one tire card from hand (Req 9)
// Returns None when hand is empty — caller must fall back to basicMove
let useTireCard (team: TeamState) : (TireType * TeamState) option =
    match team.TireCards with
    | []           -> None
    | card :: rest -> Some (card, { team with TireCards = rest })

// Replace one team in the list by Id
let updateTeam (teams: TeamState list) (updated: TeamState) : TeamState list =
    teams |> List.map (fun t -> if t.Id = updated.Id then updated else t)

// Advance TurnIndex; wraps to next round when all teams in TurnOrder have moved
// Weather update for new rounds is handled by the game loop (keeps this pure)
let advanceTurn (state: GameState) : GameState =
    let nextIdx = state.TurnIndex + 1
    if nextIdx >= List.length state.TurnOrder then
        { state with TurnIndex = 0; CurrentRound = state.CurrentRound + 1 }
    else
        { state with TurnIndex = nextIdx }

// True when TurnIndex just wrapped to 0 and this is not round 1 (Req 12)
// Use this in the game loop to know when to roll new weather
let isNewRound (state: GameState) : bool =
    state.TurnIndex = 0 && state.CurrentRound > 1

// Effective position for standings: pit teams are off-track (Position = 20)
// but have already passed pos 18, so rank them as if at TrackLength - 1.
let private racePos (t: TeamState) =
    if t.InPit then TrackLength - 1 else t.Position

// Standings: finished teams first (by finishRank), then racing teams by
// laps desc then effective position desc (Req 8, 19)
let computeStandings (state: GameState) : (TeamState * int) list =
    let finished =
        state.Teams
        |> List.filter (fun t -> t.Finished)
        |> List.sortBy (fun t -> t.FinishRank |> Option.defaultValue 999)
    let racing =
        state.Teams
        |> List.filter (fun t -> not t.Finished)
        |> List.sortWith (fun a b ->
            let cmp = compare b.Lap a.Lap
            if cmp <> 0 then cmp else compare (racePos b) (racePos a))
    List.append finished racing |> List.mapi (fun i t -> t, i + 1)

// Assign finish rank = (number of already-finished teams) + 1 (Req 20)
let finishTeam (team: TeamState) (state: GameState) : TeamState =
    let rank =
        state.Teams
        |> List.filter (fun t -> t.Finished)
        |> List.length
        |> (+) 1
    { team with Finished = true; FinishRank = Some rank }

// Race ends when every team has finished (Req 21)
let raceOver (state: GameState) : bool =
    state.Teams |> List.forall (fun t -> t.Finished)
