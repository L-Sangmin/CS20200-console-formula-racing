module Movement

open Types

// Movement values from implementation guide
let tireMove (tire: TireType) (weather: Weather) : int =
    match tire, weather with
    | Soft,   Sunny -> 9
    | Soft,   Rainy -> 4
    | Medium, Sunny -> 7
    | Medium, Rainy -> 4
    | Hard,   Sunny -> 6
    | Hard,   Rainy -> 3

// Basic movement when a team has no tire cards left (Req 13)
let basicMove (weather: Weather) : int =
    match weather with
    | Sunny -> 3
    | Rainy -> 2

// Passive bonus applied on top of base movement
// WarmTires    : Soft tires +2 in any weather
// RainEngineer : any tire +2 in Rainy
// Endurance    : basic movement (tireOpt=None) +2
// PitCrew      : no movement bonus (card-count bonus applied in Pit.fs)
let applyPassive
        (passive  : PassiveAbility)
        (tireOpt  : TireType option)
        (weather  : Weather)
        (baseMove : int) : int =
    match passive, tireOpt, weather with
    | WarmTires,    Some Soft, _     -> baseMove + 2
    | RainEngineer, _,         Rainy -> baseMove + 2
    | Endurance,    None,      _     -> baseMove + 2
    | _                              -> baseMove

// Compute final movement for a team's turn
// tireOpt = None means the team has no cards and uses basic movement
let computeMove
        (passive : PassiveAbility)
        (tireOpt : TireType option)
        (weather : Weather) : int =
    let base_ =
        match tireOpt with
        | Some tire -> tireMove tire weather
        | None      -> basicMove weather
    applyPassive passive tireOpt weather base_

// True when move crosses PitEntryPos but does NOT complete the lap (Req 14).
// If the move also reaches or passes TrackLength, the team crosses S/F instead
// and the pit option does not apply (they'd enter pit on the next lap).
let passesPitEntry (pos: int) (move: int) : bool =
    let newPos = pos + move
    pos < PitEntryPos && newPos >= PitEntryPos && newPos < TrackLength
