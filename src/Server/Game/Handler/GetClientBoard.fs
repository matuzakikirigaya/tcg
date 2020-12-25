module Server.Game.Handler.SendClientBoard

open FSharp.Control.Tasks.V2

open Saturn
open Thoth.Json
open Giraffe.Core

open Shared.Model.WebSocket
open Thoth.Json.Net
open Microsoft.AspNetCore.Http
open Channels
open Server.Game.Dummy

open Shared.Model.Game.Board

let sendClientBoard1 (hub: Channels.ISocketHub) socketId (payload: ClientBoard) =
    task {
        let payload = Encode.Auto.toString (0, payload)
        do! hub.SendMessageToClient "/channel" socketId ClientSinkApi.GetTopicName.GotGameBoard payload
    }

let sendClientBoard (ctx: HttpContext) clientInfo (message: Message<obj>) =
    task {
        let hub = ctx.GetService<Channels.ISocketHub>()

        let m =
            covertServerBoardIntoClientBoardFor1 initialServerBoard
        // Here we handle any websocket client messages in a type-safe manner
        do! sendClientBoard1 hub clientInfo.SocketId m
    }
