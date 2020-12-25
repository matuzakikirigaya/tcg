
module Server.Game.Handler.DevInit

open FSharp.Control.Tasks.V2

open Saturn
open Thoth.Json
open Giraffe.Core

open Shared.Model.WebSocket
open Thoth.Json.Net
open Microsoft.AspNetCore.Http
open Channels
open Server.Game.Dummy
open Server.Game.Program

open Shared.Model.Game.Board
open Shared.Model.Game.ClientApi.Draw

let sendClientsBoard1 (hub: Channels.ISocketHub) (payload: ClientBoard) =
    task {
        let payload = Encode.Auto.toString (0, payload)
        do! hub.SendMessageToClients "/channel" ClientSinkApi.GetTopicName.GotGameBoard payload
    }

let DevInitHandler (ctx: HttpContext) clientInfo (message: Message<obj>) =
    task {
        let hub = ctx.GetService<Channels.ISocketHub>()

        program.dispatch (Shared.Model.Game.GameElmish.DevInit)

        let m =
            covertServerBoardIntoClientBoardFor1 program.getModel.board
        // Here we handle any websocket client messages in a type-safe manner
        do! sendClientsBoard1 hub m
    }
