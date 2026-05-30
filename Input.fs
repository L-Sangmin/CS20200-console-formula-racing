module Input

open Types

let (|ParseInt|_|) (s: string) =
    match System.Int32.TryParse(s) with
    | true, n -> Some n
    | _       -> None

// Repeat until user enters int in [lo, hi]
let promptInt (prompt: string) (lo: int) (hi: int) : int =
    let rec loop () =
        printf "%s (%d–%d): " prompt lo hi
        match System.Console.ReadLine() with
        | ParseInt n when n >= lo && n <= hi -> n
        | _ ->
            printfn " ! Enter a number between %d and %d." lo hi
            loop ()
    loop ()

// Display numbered list, return chosen item
let promptChoice<'a> (prompt: string) (items: 'a list) (label: 'a -> string) : 'a =
    printfn "%s" prompt
    items |> List.iteri (fun i x -> printfn "  %d. %s" (i + 1) (label x))
    let n = promptInt "Choice" 1 (List.length items)
    List.item (n - 1) items
