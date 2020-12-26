module Server.Game.Handler.JoinWebSocket

open FSharp.Control.Tasks.V2
open Microsoft.AspNetCore.Http
open Giraffe.Core
open Saturn

open Microsoft.Extensions.Logging
open Shared.Model.WebSocket
open Thoth.Json.Net

let joinWebSocket (ctx: HttpContext) (socketId: Channels.ClientInfo) =
    task {
        ctx
            .GetLogger()
            .LogInformation("Client has connected. They've been assigned socket Id: {socketId}", socketId)

        return Channels.Ok
    }
