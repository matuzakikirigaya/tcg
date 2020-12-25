module Server.Game.Handler.JoinWebSocket

open FSharp.Control.Tasks.V2
open Microsoft.AspNetCore.Http
open Giraffe.Core
open Saturn

open Microsoft.Extensions.Logging
open Shared.Model.WebSocket
open Thoth.Json.Net

let sendMessage (hub: Channels.ISocketHub) socketId (payload: ChatSubstance) =
    task {
        let payload = Encode.Auto.toString (0, payload)
        do! hub.SendMessageToClient "/channel" socketId "" payload
    }

let joinWebSocket (ctx: HttpContext) (socketId: Channels.ClientInfo) =
    task {
        ctx
            .GetLogger()
            .LogInformation("Client has connected. They've been assigned socket Id: {socketId}", socketId)

        return Channels.Ok
    }
