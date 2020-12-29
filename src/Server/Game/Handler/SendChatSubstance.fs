module Server.Game.Handler.SendChatSubstance

open FSharp.Control.Tasks.V2

open Saturn
open Thoth.Json
open Giraffe.Core

open Shared.Model.WebSocket
open Thoth.Json.Net
open Server.Game.GameCore.Room
open Microsoft.AspNetCore.Http

let sendChatSubstance (hub: Channels.ISocketHub) (socketId) (payload: ChatSubstance) =
    task {
        let payload = Encode.Auto.toString (0, payload)
        do! hub.SendMessageToClient "/channel" socketId ClientSinkApi.GetTopicName.ReceivedChatSubstance payload
    }

let broadcastChatSubstance (hub: Channels.ISocketHub) (payload: ChatSubstance) =
    task {
        let payload = Encode.Auto.toString (0, payload)
        do! hub.SendMessageToClients "/channel" ClientSinkApi.GetTopicName.ReceivedChatSubstance payload
    }

let BroadcastChatSubstanceHandler =
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

let SendChatSubstanceHandler =
    (fun (ctx: HttpContext) (channelInfo: Channels.ClientInfo) (message: Channels.Message<obj>) ->
        task {
            let hub = ctx.GetService<Channels.ISocketHub>()

            let message =
                message.Payload
                |> string
                |> Decode.Auto.unsafeFromString<ChatSubstance>

            // ここの処理、ここに書くよりもサーバーボードのメソッドにするべきな気がしないこともない
            match gameRooms.findRoom message.userName with
            | None -> () // ここらへんの処理、Msgと合わせてエラー処理にするべきじゃん
            | Some room ->
                match room with
                | FindOne _ -> () // ここらへんの処理、Msgと合わせてエラー処理にするべきじゃん
                | Matching room ->
                    match room.getModel.board.serverPlayer1.serverPlayerSocketId with
                    | Some id -> do! sendChatSubstance hub id message
                    | _ -> () // ここらへんの処理、Msgと合わせてエラー処理にするべきじゃん

                    match room.getModel.board.serverPlayer2.serverPlayerSocketId with
                    | Some id -> do! sendChatSubstance hub id message
                    | _ -> () // ここらへんの処理、Msgと合わせてエラー処理にするべきじゃん
        // Here we handle any websocket client messages in a type-safe manner
        })
