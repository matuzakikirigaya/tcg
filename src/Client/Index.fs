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

open System

open Client.Game.WebSocket

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

    let cmd =
        Cmd.OfAsync.perform todosApi.getTodos () (TodoMsg << GotTodo)

    let navigatorModel: NavigatorModel =
        { CurrentPage = TodoPage
          User = userName }

    let webSocketModel, _ = webSocketinit userName

    ({ NavigatorModel = navigatorModel
       TodoModel = todoModel
       LoginModel = loginModel
       WebSocketModel =
           { webSocketModel with
                 GameModel =
                     { webSocketModel.GameModel with
                           PlayerName = userName } }
       CurrentPage = TodoPage }),
    cmd

let update (msg: Msg) (model: Model): Model * Cmd<Msg> = model.Update msg

open Fable.React
open Fable.React.Props

let view (model: Model) (dispatch: Msg -> unit) =
    let CurrentPage = model.CurrentPage

    div [ Class "index" ] [
        navigatorView (
            { NavigatorModel = model.NavigatorModel
              NavigatorDispatch = (dispatch << NavigatorMsg) }
        )
        hr []
        div [ Class "page" ] [
            match CurrentPage with
            | TodoPage ->
                yield
                    todoView
                        { TodoModel = model.TodoModel
                          TodoDispatch = (dispatch << TodoMsg) }
            | LoginPage ->
                yield
                    loginView
                        { loginModel = model.LoginModel
                          loginDispatch = (dispatch << LoginMsg) }
            | chatPage ->
                yield
                    WebSocketMsg
                    >> dispatch
                    |> model.WebSocketModel.View
        ]
    ]
