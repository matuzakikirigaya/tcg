module Client.Game.WebSocket

open Shared.Model.WebSocket

open Client.Game.Chat
open Client.Game.Field
open Elmish
open Fable.React.Props
open Fable.React
open Client.Game.Field

type ClientSender = ClientApi -> unit

type ConnectionState =
    | DisConnected
    | Connected of ClientSender
    | Connecting
    member this.SendChatSubsatnce sub =
        match this with
        | Connected sender -> sender <| SendChatSubstance sub
        | _ -> ()

    member this.GetGameBoard() =
        match this with
        | Connected sender -> sender GetGameBoard
        | _ -> ()

type WebSocketMsg =
    | ChatMsg of ChatMsg
    | GameMsg of GameMsg
    | MConnect of ConnectionState

type WebSocketModel =
    { ConnectionState: ConnectionState
      ChatModel: ChatModel
      GameModel: GameModel }
    member This.Update(msg: WebSocketMsg): WebSocketModel * Cmd<WebSocketMsg> =
        match msg with
        | ChatMsg chatmsg ->
            let (chatModel, chatCmd) = This.ChatModel.Update chatmsg
            { This with ChatModel = chatModel }, Cmd.map ChatMsg chatCmd
        | GameMsg gamemsg ->
            let (gameModel, gameCmd) = This.GameModel.Update gamemsg
            { This with GameModel = gameModel }, Cmd.map GameMsg gameCmd
        | MConnect connection ->
            { This with
                  ConnectionState = connection
                  ChatModel =
                      { This.ChatModel with
                            ChatSender = connection.SendChatSubsatnce }
                  GameModel =
                      { This.GameModel with
                            GameSender = connection.GetGameBoard } },
            Cmd.none

    member This.View(dispatch: WebSocketMsg -> unit): ReactElement =
        div [ Class "game" ] [
            This.GameModel.View(GameMsg >> dispatch)
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
            {| Topic = ClientApi.GetTopicNameWithApi clientapi
               Ref = ""
               Payload = message |}

        let message =
            Thoth.Json.Encode.Auto.toString (0, message)

        ws.send message
    | GetGameBoard ->
        let message =
            {| Topic = ClientApi.GetTopicName.GetGameBoard |}

        let message =
            Thoth.Json.Encode.Auto.toString (0, message)

        ws.send message

open Browser.WebSocket
open Browser.Types
open Shared.Model.Game.Board

let subscription _ =
    let sub (dispatch: WebSocketMsg -> unit) =
        /// Handles push messages from the server and relays them into Elmish messages.
        let onWebSocketMessage (msg: MessageEvent) =
            let msg =
                msg.data
                |> decode<{| payload: string; topic: string |}>

            if msg.topic = ClientApi.GetTopicName.SendChatSubstance then
                msg.payload
                |> decode<ChatSubstance>
                |> MReceivedSubstance
                |> ChatMsg
                |> dispatch
            else if msg.topic = ClientApi.GetTopicName.GetGameBoard then
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
