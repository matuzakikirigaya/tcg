module Client.Game.WebSocket

open Shared.Model.WebSocket

open Client.Game.Chat

type ClientSender = ClientApi -> unit

type ConnectionState =
    | DisConnected
    | Connected of ClientSender
    | Connecting
    member this.SendChatSubsatnce sub =
        match this with
        | Connected sender -> sender <| SendChatSubstance sub
        | _ -> ()

type WebSocketMsg =
    | ChatMsg of ChatMsg
    | MConnect of ConnectionState

open Elmish
open Fable.React.Props
open Fable.React
open Client.Game.Field

type WebSocketModel =
    { ConnectionState: ConnectionState
      ChatModel: ChatModel
      GameModel: GameModel }
    member This.Update(msg: WebSocketMsg): WebSocketModel * Cmd<WebSocketMsg> =
        match msg with
        | ChatMsg chatmsg ->
            let (one, two) = This.ChatModel.Update chatmsg
            { This with ChatModel = one }, Cmd.map ChatMsg two
        | MConnect connection ->
            { This with
                  ConnectionState = connection
                  ChatModel =
                      { This.ChatModel with
                            ChatSender = connection.SendChatSubsatnce } },
            Cmd.none

    member This.View(dispatch: WebSocketMsg -> unit): ReactElement =
        div [ Class "game" ] [
            This.GameModel.View()
            hr []
            This.ChatModel.view (ChatMsg >> dispatch)
        ]

let webSocketinit initialUserName =
    { ConnectionState = DisConnected
      ChatModel =
          { ChatSender = fun a -> ()
            ReceivedSubstance = []
            SendingSubstance =
                { substance = ""
                  userName = initialUserName }
            UserName = initialUserName }
      GameModel = gameModelInit },
    Cmd.none

open Browser.WebSocket
open Browser.Types

let inline decode<'a> x =
    x
    |> unbox<string>
    |> Thoth.Json.Decode.Auto.unsafeFromString<'a>

let buildClientSender (ws: WebSocket) clientapi =
    match clientapi with
    | SendChatSubstance message ->
        let message =
            {| Topic = "sendChatSubstance"
               Ref = ""
               Payload = message |}

        let message =
            Thoth.Json.Encode.Auto.toString (0, message)

        ws.send message

open Browser.WebSocket
open Browser.Types

let subscription _ =
    let sub (dispatch: WebSocketMsg -> unit) =
        /// Handles push messages from the server and relays them into Elmish messages.
        let onWebSocketMessage (msg: MessageEvent) =
            let msg =
                msg.data
                |> decode<{| payload: string; topic: string |}>

            if msg.topic = "sendChatSubstance" then
                msg.payload
                |> decode<ChatSubstance>
                |> MReceivedSubstance
                |> ChatMsg
                |> dispatch
            else
                ()


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
