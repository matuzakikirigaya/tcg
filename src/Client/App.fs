module App

open Elmish
open Elmish.React

#if DEBUG
open Elmish.Debug
open Elmish.HMR
#endif

Program.mkProgram Index.Program.init Index.Program.update Index.Program.view
#if DEBUG
|> Program.withConsoleTrace
|> Program.withDebugger
#endif
|> Program.withReactSynchronous "elmish-app"
#if DEBUG
#endif
|> Program.run
