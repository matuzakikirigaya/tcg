module Server.WebSocket

open Saturn
open FSharp.Control.Tasks.V2
open Thoth.Json
open Giraffe.Core

open Microsoft.Extensions.Logging
open Shared.Model.WebSocket
open Thoth.Json.Net

open Server.Game.Dummy
/// Provides some simple functions over the ISocketHub interface.
let sendMessage (hub: Channels.ISocketHub) socketId (payload: ChatSubstance) =
    task {
        let payload = Encode.Auto.toString (0, payload)
        do! hub.SendMessageToClient "/channel" socketId "" payload
    }
/// Sends a message to all connected clients.

open Shared.Model.Game.Board

let sendClientBoard (hub: Channels.ISocketHub) socketId (payload: ClientBoard) =
    task {
        let payload = Encode.Auto.toString (0, payload)
        do! hub.SendMessageToClient "/channel" socketId ClientSinkApi.GetTopicName.GotGameBoard payload
    }

open Server.Game.Handler.SendChatSubstance
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

        handle ClientSourceApi.GetTopicName.SendChatSubstance SendChatSubstanceHandler

        handle
            ClientSourceApi.GetTopicName.GetGameBoard
            (fun ctx clientInfo message ->
                task {
                    let hub = ctx.GetService<Channels.ISocketHub>()

                    let m =
                        covertServerBoardIntoClientBoardFor1 initialServerBoard
                    // Here we handle any websocket client messages in a type-safe manner
                    do! sendClientBoard hub clientInfo.SocketId m
                })
    }
