module Server.Game.Handler.SendClientBoard

open FSharp.Control.Tasks.V2

open Saturn
open Thoth.Json
open Giraffe.Core

open Thoth.Json.Net
open Microsoft.AspNetCore.Http
open Channels
open Server.Game.Program
open Shared.Model.Game.ClientApi.SimplyName
open Shared.Model.Game.Util.BoardUtil
open Server.Game.GameCore.Room

open Server.Game.Handler.SendClientBoardi

let sendClientBoard (ctx: HttpContext) clientInfo (message: Message<obj>) =
    task {
        let hub = ctx.GetService<Channels.ISocketHub>()

        let message =
            message.Payload
            |> string
            |> Decode.Auto.unsafeFromString<SimplyName>

        let programi =
            gameRooms.matchWithNewPlayer (message.playerName)

        match programi with
        | Matching programi ->
            programi.dispatch (
                Shared.Model.Game.GameElmish.SGetGameBoard
                    { playerName = message.playerName
                      socketId = clientInfo.SocketId }
            )

            let board = programi.getModel.board

            let m1 =
                soroeruServerBoardByName message.playerName board
            // Here we handle any websocket client messages in a type-safe manner
            do! sendClientBoard2 hub m1
        | FindOne player -> return ()
    }
