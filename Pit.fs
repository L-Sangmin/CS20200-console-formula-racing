module Pit

open Types

// Card counts from implementation guide (Req 16)
let tireCardCount (tire: TireType) : int =
    match tire with
    | Soft   -> 3
    | Medium -> 5
    | Hard   -> 10

// Branch into pit lane: store team off-track (Position = PitSpacePos = 20)
// and replenish tire cards. PitCrew passive grants +2 bonus cards. (Req 15, 16)
let enterPit (team: TeamState) (tire: TireType) : TeamState =
    let baseCount = tireCardCount tire
    let count =
        match team.Passive with
        | PitCrew -> baseCount + 2
        | _       -> baseCount
    { team with
        Position  = PitSpacePos       // off-track sentinel (= 20)
        InPit     = true
        TireCards = List.replicate count tire }

// Exit pit lane: team rejoins at pos 0 (S/F line) and the lap counts as complete.
// Caller (game loop) then applies the tire-card movement from pos 0. (Req 17, 18)
let exitPit (team: TeamState) : TeamState =
    { team with
        InPit    = false
        Lap      = team.Lap + 1   // crossing S/F at pit exit = +1 lap
        Position = 0 }
