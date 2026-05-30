module Movement

open Types

// Movement values from implementation guide
let tireMove (tire: TireType) (weather: Weather) : int =
    match tire, weather with
    | Soft,   Sunny -> 7
    | Soft,   Rainy -> 4
    | Medium, Sunny -> 6
    | Medium, Rainy -> 3
    | Hard,   Sunny -> 5
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

// Per-card base move: variance applies only in Sunny (Rainy has fixed values)
let cardMove (card: TireCard) (weather: Weather) : int =
    let v = if weather = Rainy then 0 else card.Variance
    max 1 (tireMove card.Tire weather + v)

// Compute final movement for a team's turn.
// cardOpt = None means no cards → basic movement.
// Passive bonus applied on top of card base.
let computeMove
        (passive : PassiveAbility)
        (cardOpt : TireCard option)
        (weather : Weather) : int =
    let tireOpt = cardOpt |> Option.map (fun c -> c.Tire)
    let base_ =
        match cardOpt with
        | Some card -> cardMove card weather
        | None      -> basicMove weather
    applyPassive passive tireOpt weather base_

// Roll variance delta with 1:2:1 distribution → -1 / 0 / +1
let rollVariance (rng: System.Random) : int =
    match rng.Next(4) with
    | 0 -> -1
    | 3 ->  1
    | _ ->  0

// True when move crosses PitEntryPos.
// Pit entry is offered even when the move also crosses S/F; pit exit handles lap counting.
let passesPitEntry (pos: int) (move: int) : bool =
    let newPos = pos + move
    pos < PitEntryPos && newPos >= PitEntryPos
