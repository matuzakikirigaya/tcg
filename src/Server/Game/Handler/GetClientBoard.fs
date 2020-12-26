module Server.Game.Handler.SendClientBoard

open FSharp.Control.Tasks.V2

open Saturn
open Thoth.Json
open Giraffe.Core

open Shared.Model.WebSocket
open Thoth.Json.Net
open Microsoft.AspNetCore.Http
open Channels
open Server.Game.Program
open Shared.Model.Game.ClientApi.SimplyName
open Shared.Model.Game.Util.BoardUtil

open Shared.Model.Game.Board
open Server.Game.Handler.SendClientBoardi

let sendClientBoard (ctx: HttpContext) clientInfo (message: Message<obj>) =
    task {
        let hub = ctx.GetService<Channels.ISocketHub>()

        let message =
            message.Payload
            |> string
            |> Decode.Auto.unsafeFromString<SimplyName>
        program.dispatch (
            Shared.Model.Game.GameElmish.SGetGameBoard
                { playerName = message.playerName
                  socketId = clientInfo.SocketId }
        )

        let board = program.getModel.board
        let m1 =
            soroeruServerBoardByName message.playerName board
        // Here we handle any websocket client messages in a type-safe manner
        sendClientBoard2 hub m1
    }
