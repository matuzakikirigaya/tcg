module Index.MM

open Elmish
open Fable.Remoting.Client
open Pages.Todo
open Pages.Login
open Pages.Navigator
open Shared

open Client.Utils.Msg
open Client.Game.WebSocket

type Msg =
    | NavigatorMsg of NavigatorMsg
    | TodoMsg of TodoMsg
    | LoginMsg of LoginMsg
    | WebSocketMsg of WebSocketMsg

type Model =
    { NavigatorModel: NavigatorModel
      TodoModel: TodoModel
      LoginModel: LoginModel
      WebSocketModel: WebSocketModel
      CurrentPage: CurrentPage }
    member this.Update msg =
        match msg with
        | TodoMsg todoMsg ->
            let soModel, soCmd = this.TodoModel.Update todoMsg
            let siCmd = Cmd.map TodoMsg soCmd
            { this with TodoModel = soModel }, siCmd
        | LoginMsg loginMsg ->
            let soModel, soCmd = this.LoginModel.Update loginMsg

            let siCmd = Cmd.map LoginMsg soCmd

            let user =
                match soModel.UserData with
                | Some u -> u.userName
                | _ -> this.NavigatorModel.User

            { this with
                  LoginModel = soModel
                  NavigatorModel = { this.NavigatorModel with User = user }
                  WebSocketModel =
                      { this.WebSocketModel with
                            ChatModel =
                                { this.WebSocketModel.ChatModel with
                                      UserName = user }
                            GameModel = {this.WebSocketModel.GameModel with PlayerName = user} }
                   },
            siCmd
        | WebSocketMsg webSocketMsg ->
            let soModel, soCmd = this.WebSocketModel.Update webSocketMsg
            let siCmd = Cmd.map WebSocketMsg soCmd
            { this with WebSocketModel = soModel }, siCmd
        | NavigatorMsg navigatorMsg ->
            let soModel, soCmd = this.NavigatorModel.Update navigatorMsg

            let siCmd = Cmd.map NavigatorMsg soCmd

            { this with
                  NavigatorModel = soModel
                  CurrentPage = soModel.CurrentPage },
            siCmd

// NavigatorMsg
