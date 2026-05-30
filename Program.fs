module Program

[<EntryPoint>]
let main _ =
    let state = Setup.setupGame ()
    Render.renderAll state
    0
