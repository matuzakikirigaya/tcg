module App

open Elmish
open Elmish.React

#if DEBUG
open Elmish.Debug
open Elmish.HMR
#endif

open Index.MM
open Pages.Chat

Program.mkProgram Index.Program.init Index.Program.update Index.Program.view
#if DEBUG
|> Program.withConsoleTrace
|> Program.withDebugger
#endif
|> Program.withSubscription (fun x -> subscription x |> Cmd.map Index.MM.WebSocketMsg)
|> Program.withReactSynchronous "elmish-app"
#if DEBUG
#endif
|> Program.run
