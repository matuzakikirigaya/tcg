module Server.Game.Handler.Draw

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
open Shared.Model.Game.ClientApi.SimplyName
open Shared.Model.Game.Util.BoardUtil

open Server.Game.Handler.SendClientBoardi

let drawHandler (ctx: HttpContext) clientInfo (message: Message<obj>) =
    task {
        let hub = ctx.GetService<Channels.ISocketHub>()

        let message =
            message.Payload
            |> string
            |> Decode.Auto.unsafeFromString<SimplyName>

        program.dispatch (Shared.Model.Game.GameElmish.Draw message)

        let board = program.getModel.board
        // Here we handle any websocket client messages in a type-safe manner
        do! sendClientBoard2 hub board
    }
