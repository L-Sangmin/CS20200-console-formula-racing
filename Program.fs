module Program

open Types

// Demo state: 3 teams at start line, round 1, Sunny
// Used to verify Render output before Setup/AI are implemented
let private demoState : GameState = {
    Teams = [
        { Id = 1; Name = "Alpha"; Kind = Human;  Passive = WarmTires;
          Position = 0; Lap = 0; TireCards = [ Soft; Soft; Soft ];
          InPit = false; Finished = false; FinishRank = None }
        { Id = 2; Name = "Beta";  Kind = AI;     Passive = RainEngineer;
          Position = 9; Lap = 0; TireCards = [ Medium; Medium; Medium; Medium; Medium ];
          InPit = false; Finished = false; FinishRank = None }
        { Id = 3; Name = "Gamma"; Kind = AI;     Passive = PitCrew;
          Position = PitSpacePos; Lap = 0; TireCards = [ Hard; Hard; Hard ];
          InPit = true; Finished = false; FinishRank = None }
    ]
    TurnOrder    = [ 1; 2; 3 ]
    CurrentRound = 1
    Weather      = Sunny
    TargetLaps   = TargetLaps
    TurnIndex    = 0
}

[<EntryPoint>]
let main _ =
    printfn "=== Console Formula Racing — Render Demo ==="
    Render.renderAll demoState
    0
