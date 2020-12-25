module Server.Game.Handler.SendChatSubstance

open FSharp.Control.Tasks.V2

open Saturn
open Thoth.Json
open Giraffe.Core

open Shared.Model.WebSocket
open Thoth.Json.Net
open Microsoft.AspNetCore.Http

let broadcastChatSubstance (hub: Channels.ISocketHub) (payload: ChatSubstance) =
    task {
        let payload = Encode.Auto.toString (0, payload)
        do! hub.SendMessageToClients "/channel" ClientSinkApi.GetTopicName.ReceivedChatSubstance payload
    }
let SendChatSubstanceHandler =
    (fun (ctx: HttpContext) (channelInfo: Channels.ClientInfo) (message: Channels.Message<obj>) ->
        task {
            let hub = ctx.GetService<Channels.ISocketHub>()

            let message =
                message.Payload
                |> string
                |> Decode.Auto.unsafeFromString<ChatSubstance>

            // Here we handle any websocket client messages in a type-safe manner
            do! broadcastChatSubstance hub message
        })
