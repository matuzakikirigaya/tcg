module Client.Game.WebSocket

open Shared.Model.WebSocket

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
    | MSubmitSubstance
    | MChangeSubstance of ChatSubstance
    | MReceivedSubstance of ChatSubstance
    | MConnect of ConnectionState

open Elmish
open Fable.React.Props
open Fable.React
open Client.Game.Field

type WebSocketModel =
    { ConnectionState: ConnectionState
      ReceivedSubstance: list<ChatSubstance>
      SendingSubstance: ChatSubstance
      UserName: string
      GameModel: GameModel }
    member This.Update(msg: WebSocketMsg): WebSocketModel * Cmd<WebSocketMsg> =
        match msg with
        | MSubmitSubstance ->
            This.ConnectionState.SendChatSubsatnce This.SendingSubstance
            This, Cmd.none
        | MChangeSubstance sub -> { This with SendingSubstance = sub }, Cmd.none
        | MReceivedSubstance sub ->
            { This with
                  ReceivedSubstance = sub :: This.ReceivedSubstance },
            Cmd.none
        | MConnect connection ->
            { This with
                  ConnectionState = connection },
            Cmd.none

    member This.View(dispatch: WebSocketMsg -> unit): ReactElement =
        div [ Class "game" ] [
            Client.Game.Field.view This.GameModel.ClientBoard
            hr []
            div [] [
                div [ ClassName "sub_title" ] [
                    (match This.ConnectionState with
                     | DisConnected -> str "dis"
                     | _ -> str "connected")
                ]
                input [ OnChange
                            (fun ev ->
                                MChangeSubstance
                                    { This.SendingSubstance with
                                          substance = (ev.Value)
                                          userName = This.UserName }
                                |> dispatch) ]
                button [ OnMouseDown(fun ev -> dispatch MSubmitSubstance)
                         ClassName "msr_btn13" ] [
                    str "submit"
                ]
                div []
                <| List.map
                    (fun value ->
                        div [] [
                            str <| value.userName + ":"
                            br []
                            div [ Class "substance_right" ] [
                                str value.substance
                            ]
                        ])
                    This.ReceivedSubstance
            ]
        ]

let webSocketinit initialUserName =
    { ConnectionState = DisConnected
      ReceivedSubstance = []
      SendingSubstance =
          { substance = ""
            userName = initialUserName }
      UserName = initialUserName
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
    let sub dispatch =
        /// Handles push messages from the server and relays them into Elmish messages.
        let onWebSocketMessage (msg: MessageEvent) =
            let msg =
                msg.data
                |> decode<{| payload: string; topic: string |}>

            if msg.topic = "sendChatSubstance" then
                msg.payload
                |> decode<ChatSubstance>
                |> MReceivedSubstance
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
