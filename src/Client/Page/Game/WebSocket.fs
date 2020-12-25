module Client.Game.WebSocket

open Shared.Model.WebSocket

open Client.Game.Chat
open Client.Game.Field
open Elmish
open Fable.React.Props
open Fable.React
open Client.Game.Field

type ClientSender = ClientSourceApi -> unit

type ConnectionState =
    | DisConnected
    | Connected of ClientSender
    | Connecting

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
            match connection with
            | Connected sender ->
                { This with
                      ConnectionState = connection
                      ChatModel =
                          { This.ChatModel with
                                ChatSender = sender << SendChatSubstance }
                      GameModel =
                          { This.GameModel with
                                GameSender = sender << GameApi } },
                Cmd.none
            | _ -> This,Cmd.none

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
