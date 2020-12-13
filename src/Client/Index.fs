module Index.Program

open Elmish
open Fable.Remoting.Client
open Pages.Todo
open Pages.Login
open Pages.Navigator
open Shared
open Index.MM

open Shared.Model.Login
open Thoth.Json
open LocalStorage

open Client.Utils.Msg
open System

open Pages.Chat

let init (): Model * Cmd<Msg> =
    let todoModel = { Todos = []; Input = "" }

    let loadUser (): UserData option =
        let userDecoder = Decode.Auto.generateDecoder<UserData> ()
        match LocalStorage.load userDecoder "user" with
        | Ok user -> Some user
        | Error _ -> None

    let user = loadUser ()

    let userName =
        match user with
        | Some u -> u.userName
        | _ -> "guesty"

    let loginModel =
        { login =
              { userName = userName
                password = ""
                passwordId = Guid.NewGuid() }
          ErrorMsg = None
          IsRunning = false
          UserData = None }

    let msgFactory = MsgFactory()

    let cmd =
        Cmd.OfAsync.perform todosApi.getTodos () (msgFactory.SiTodoMsg << GotTodo)

    let navigatorModel: NavigatorModel =
        { CurrentPage = TodoPage
          User = userName }

    let clientChatModel, _ = chatInit ()

    ({ NavigatorModel = navigatorModel
       TodoModel = todoModel
       LoginModel = loginModel
       ClientChatModel = clientChatModel
       CurrentPage = TodoPage }),
    cmd

let update (msg: Msg) (model: Model): Model * Cmd<Msg> = msg.Update model
open Fable.React

let view (model: Model) (dispatch: Msg -> unit) =
    let CurrentPage = model.CurrentPage
    let msgFactory = MsgFactory()
    div [] [
        navigatorView
            ({ NavigatorModel = model.NavigatorModel
               NavigatorDispatch = (dispatch << msgFactory.SiNavigatorMsg) })
        hr []
        div [] [
            match CurrentPage with
            | TodoPage ->
                yield
                    todoView
                        { TodoModel = model.TodoModel
                          TodoDispatch = (dispatch << msgFactory.SiTodoMsg) }
            | LoginPage ->
                yield
                    loginView
                        { loginModel = model.LoginModel
                          loginDispatch = (dispatch << msgFactory.SiLoginMsg) }
            | chatPage ->
                yield
                    chatView
                        {| clientChatModel = model.ClientChatModel
                           dispatch = (dispatch << msgFactory.SiClientChatMsg) |}
        ]
    ]
