module Setup

open Types

let private rng = System.Random()

let private allPassives = [ WarmTires; RainEngineer; PitCrew; Endurance ]
let private allTires    = [ Soft; Medium; Hard ]

let private tireDetailLabel (t: TireType) : string =
    let sunny = Movement.tireMove t Sunny
    let rainy = Movement.tireMove t Rainy
    let cards = Pit.tireCardCount t
    sprintf "%-6s  Sunny:%d  Rainy:%d  Cards:%d" (Render.tireLabel t) sunny rainy cards

let private makeTeam (id: int) (name: string) (kind: TeamKind)
                     (passive: PassiveAbility) (tire: TireType) : TeamState =
    let count =
        let base_ = Pit.tireCardCount tire
        match passive with PitCrew -> base_ + 2 | _ -> base_
    { Id         = id
      Name       = name
      Kind       = kind
      Passive    = passive
      Position   = 0
      Lap        = 0
      TireCards  = List.replicate count tire
      InPit      = false
      Finished   = false
      FinishRank = None }

let private aiNames = [| "Redbull"; "Ferrari"; "Mercedes"; "McLaren"; "Alpine" |]

let private sep = String.replicate 50 "-"

let setupGame () : GameState =
    printfn ""
    printfn "%s" sep
    printfn "  Console Formula Racing — Setup"
    printfn "%s" sep

    // Number of AI opponents (total teams = 1 human + n AI)
    let aiCount = Input.promptInt " How many AI opponents?" 1 3

    printfn "%s" sep

    // Human passive
    let humanPassive =
        Input.promptChoice " Choose your passive ability:" allPassives Render.passiveLabel

    printfn "%s" sep

    // Human starting tire
    let humanTire =
        Input.promptChoice " Choose your starting tire:" allTires tireDetailLabel

    // AI teams — random passive, random tire (excluding human passive to avoid copies when possible)
    let usedPassives = System.Collections.Generic.HashSet<PassiveAbility>([humanPassive])
    let aiTeams =
        [ 1 .. aiCount ] |> List.map (fun i ->
            let available =
                allPassives |> List.filter (fun p -> not (usedPassives.Contains p))
            let passive =
                if List.isEmpty available then
                    allPassives.[rng.Next(List.length allPassives)]
                else
                    available.[rng.Next(List.length available)]
            usedPassives.Add(passive) |> ignore
            let tire = allTires.[rng.Next(List.length allTires)]
            let name = aiNames.[i - 1]
            makeTeam (i + 1) name AI passive tire)

    let humanTeam = makeTeam 1 "You" Human humanPassive humanTire

    // Randomise turn order
    let allTeams    = humanTeam :: aiTeams
    let turnOrder   =
        allTeams
        |> List.map (fun t -> t.Id)
        |> List.sortBy (fun _ -> rng.Next())

    printfn "%s" sep
    printfn "  Turn order"
    printfn "%s" sep
    turnOrder |> List.iter (fun id ->
        let t = allTeams |> List.find (fun t -> t.Id = id)
        printfn "  %s [%s]" t.Name (Render.kindLabel t.Kind))
    printfn "%s" sep

    { Teams        = allTeams
      TurnOrder    = turnOrder
      CurrentRound = 1
      Weather      = Sunny
      TargetLaps   = TargetLaps
      TurnIndex    = 0 }
