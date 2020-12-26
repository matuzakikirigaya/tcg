module Server.Game.Handler.DevInit

open FSharp.Control.Tasks.V2

open Saturn
open Thoth.Json
open Giraffe.Core

open Shared.Model.WebSocket
open Thoth.Json.Net
open Microsoft.AspNetCore.Http
open Channels
open Server.Game.Program

open Shared.Model.Game.Board
open Shared.Model.Game.Util.BoardUtil
open Shared.Model.Game.ClientApi.SimplyName

open Server.Game.Handler.SendClientBoardi

let DevInitHandler (ctx: HttpContext) clientInfo (message: Message<obj>) =
    task {
        let hub = ctx.GetService<Channels.ISocketHub>()

        program.dispatch (Shared.Model.Game.GameElmish.DevInit)

        let message =
            message.Payload
            |> string
            |> Decode.Auto.unsafeFromString<SimplyName>

        let m =
            covertServerBoardIntoClientBoardByName message.playerName program.getModel.board
        // Here we handle any websscket client messages in a type-safe manner

        do! sendClientBoard2 hub program.getModel.board
    }
