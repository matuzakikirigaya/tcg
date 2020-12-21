module Index.MM

open Elmish
open Fable.Remoting.Client
open Pages.Todo
open Pages.Login
open Pages.Navigator
open Shared

open Client.Utils.Msg
open Pages.Chat

type Model =
    { NavigatorModel: NavigatorModel
      TodoModel: TodoModel
      LoginModel: LoginModel
      WebSocketModel: WebSocketModel
      CurrentPage: CurrentPage }

type Msg = IMsg<Model>

type SiTodoMsg(todoMsg: ITodoMsg) =
    class
        let todoMsg = todoMsg

        interface Msg with
            member this.Update model =
                let soModel, soCmd = todoMsg.Update model.TodoModel

                let siCmd =
                    Cmd.map (fun todomsg -> SiTodoMsg(todomsg) :> Msg) soCmd


                { model with TodoModel = soModel }, siCmd
    end

type SiLoginMsg(loginMsg: LoginMsg) =
    class
        let loginMsg = loginMsg

        interface Msg with
            member this.Update model =
                let soModel, soCmd = loginMsg.Update model.LoginModel

                let siCmd =
                    Cmd.map (fun loginmsg -> SiLoginMsg(loginmsg) :> Msg) soCmd

                let user =
                    match soModel.UserData with
                    | Some u -> u.userName
                    | _ -> model.NavigatorModel.User

                { model with
                      LoginModel = soModel
                      NavigatorModel =
                          { model.NavigatorModel with
                                User = user } },
                siCmd
    end

type SiWebSocketMsg(chatMsg: WebSocketMsg) =
    class
        let chatMsg = chatMsg

        interface Msg with
            member this.Update model =
                let soModel, soCmd = model.WebSocketModel.Update chatMsg

                let siCmd =
                    Cmd.map (fun todomsg -> SiWebSocketMsg(todomsg) :> Msg) soCmd

                { model with WebSocketModel = soModel }, siCmd
    end

type SiNavigatorMsg(navigatorMsg: INavigatorMsg) =
    class
        let navigatorMsg = navigatorMsg

        interface Msg with
            member this.Update model =
                let soModel, soCmd = navigatorMsg.Update model.NavigatorModel

                let siCmd =
                    Cmd.map (fun navigatormsg -> SiNavigatorMsg(navigatormsg) :> Msg) soCmd

                { model with
                      NavigatorModel = soModel
                      CurrentPage = soModel.CurrentPage },
                siCmd
    end

type MsgFactory() =
    class
        member _.SiTodoMsg(todoMsg) = SiTodoMsg(todoMsg) :> Msg
        member _.SiLoginMsg(loginMsg) = SiLoginMsg(loginMsg) :> Msg
        member _.SiNavigatorMsg(navigatorMsg) = SiNavigatorMsg(navigatorMsg) :> Msg
        member _.SiWebSocketMsg(clientChatMsg) = SiWebSocketMsg(clientChatMsg) :> Msg
    end
