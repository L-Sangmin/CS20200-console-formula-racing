module Types

// Track constants
let TrackLength = 20        // spaces 0–19
let PitEntryPos = 18        // crossing this triggers pit option
let PitSpacePos = TrackLength   // = 20, off-track sentinel for teams in pit lane
let TargetLaps  = 3

type Weather =
    | Sunny
    | Rainy

type TireType =
    | Soft
    | Medium
    | Hard

// Passive ability for each team (user picks, AI gets random)
// WarmTires    : Soft tires +2 movement in any weather
// RainEngineer : all tires +2 movement in Rainy weather
// PitCrew      : +2 bonus cards on any pit replenish
// Endurance    : basic movement (no tires left) +2 above normal
type PassiveAbility =
    | WarmTires
    | RainEngineer
    | PitCrew
    | Endurance

// Used by Render to label each position on the track
type TrackSpace =
    | StartFinish
    | Normal
    | PitEntry
    | PitSpace

type TeamKind =
    | Human
    | AI

type TeamState = {
    Id         : int
    Name       : string
    Kind       : TeamKind
    Passive    : PassiveAbility
    Position   : int               // 0–19; 0 = Start/Finish line
    Lap        : int               // laps completed so far
    TireCards  : TireType list     // current hand; one card spent per turn
    InPit      : bool
    Finished   : bool
    FinishRank : int option        // assigned when team finishes
}

type GameState = {
    Teams        : TeamState list
    TurnOrder    : int list        // team Ids in order for the round
    CurrentRound : int
    Weather      : Weather
    TargetLaps   : int
    TurnIndex    : int             // index into TurnOrder for current turn
}
