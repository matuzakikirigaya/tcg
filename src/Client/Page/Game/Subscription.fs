module Client.Game.Subscription

open Client.Game.WebSocket
open Browser.WebSocket
open Browser.Types


open Browser.WebSocket
open Browser.Types
open Shared.Model.Game.Board
open Client.Game.Api.Index
open Shared.Model.WebSocket
open Elmish
open Client.Game.Chat
open Client.Game.Field

let inline decode<'a> x =
    x
    |> unbox<string>
    |> Thoth.Json.Decode.Auto.unsafeFromString<'a>
let subscription _ =
    let sub (dispatch: WebSocketMsg -> unit) =
        /// Handles push messages from the server and relays them into Elmish messages.
        let onWebSocketMessage (msg: MessageEvent) =
            let msg =
                msg.data
                |> decode<{| payload: string; topic: string |}>

            if msg.topic = ClientSourceApi.GetTopicName.SendChatSubstance then
                msg.payload
                |> decode<ChatSubstance>
                |> MReceivedSubstance
                |> ChatMsg
                |> dispatch
            else if msg.topic = ClientSourceApi.GetTopicName.GetGameBoard then
                msg.payload
                |> decode<ClientBoard>
                |> MGotBoard
                |> GameMsg
                |> dispatch


        /// Continually tries to connect to the server websocket.
        let rec connect () =
            let ws =
                WebSocket.Create "ws://localhost:8085/channel"

            ws.onmessage <- onWebSocketMessage

            ws.onopen <-
                (fun ev ->
                    buildClientSender ws
                    |> Connected
                    |> MConnect
                    |> dispatch

                    printfn "WebSocket opened")

            ws.onclose <-
                (fun ev ->
                    DisConnected |> MConnect |> dispatch
                    printfn "WebSocket closed. Retrying connection"

                    promise {
                        do! Promise.sleep 2000
                        Connecting |> MConnect |> dispatch
                        connect ()
                    })

        connect ()

    Cmd.ofSub sub
