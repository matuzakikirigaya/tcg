module Server.WebSocket

open Fable.Remoting.Server
open Fable.Remoting.Giraffe
open Saturn
open FSharp.Control.Tasks.V2
open System.IO
open Thoth.Json
open Giraffe.ModelBinding
open Giraffe.Core

open Microsoft.Extensions.Logging
open Shared.Model.WebSocket
open Thoth.Json.Net
/// Provides some simple functions over the ISocketHub interface.
let sendMessage (hub: Channels.ISocketHub) socketId (payload: ChatSubstance) =
    task {
        let payload = Encode.Auto.toString (0, payload)
        do! hub.SendMessageToClient "/channel" socketId "" payload
    }
/// Sends a message to all connected clients.
let broadcastMessage (hub: Channels.ISocketHub) (payload: ChatSubstance) =
    task {
        let payload = Encode.Auto.toString (0, payload)
        do! hub.SendMessageToClients "/channel" "sendChatSubstance" payload
    }

/// Sets up the channel to listen to clients.
let channel =
    channel {
        join
            (fun ctx socketId ->
                task {
                    ctx
                        .GetLogger()
                        .LogInformation("Client has connected. They've been assigned socket Id: {socketId}", socketId)

                    return Channels.Ok
                })

        handle
            "sendChatSubstance"
            (fun ctx channelInfo message ->
                task {
                    let hub = ctx.GetService<Channels.ISocketHub>()

                    let message =
                        message.Payload
                        |> string
                        |> Decode.Auto.unsafeFromString<ChatSubstance>

                    // Here we handle any websocket client messages in a type-safe manner
                    do! broadcastMessage hub message
                })
    }
