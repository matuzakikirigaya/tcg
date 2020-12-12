module Index.Program

open Elmish
open Fable.Remoting.Client
open Pages.Todo
open Pages.Login
open Pages.Navigator
open Shared
open Index.MM

open Client.Utils.Msg


let init (): Model * Cmd<Msg> =
    let todoModel = { Todos = []; Input = "" }
    let userName = "hogekun"
    let loginModel = { UserName = userName; Password = "" }

    let msgFactory =MsgFactory()

    let imsgfactory = ITodoMsgFactory()

    let cmd =
        Cmd.OfAsync.perform todosApi.getTodos () (msgFactory.SiTodoMsg << imsgfactory.GotTodo)

    let navigatorModel: NavigatorModel =
        { CurrentPage = TodoPage
          User = { UserName = "guest" } }

    ({ NavigatorModel = navigatorModel
       TodoModel = todoModel
       LoginModel = loginModel
       CurrentPage = TodoPage }),
    cmd

let update (msg: Msg) (model1: Model): Model * Cmd<Msg> =
    msg.Update model1
open Fable.React

let view (model: Model) (dispatch: Msg -> unit) =
    let CurrentPage = model.CurrentPage
    let msgFactory = MsgFactory()
    div [] [
        navigatorView ({ NavigatorDispatch = (dispatch << msgFactory.SiNavigatorMsg) })
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
        ]
    ]
