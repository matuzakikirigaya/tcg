module Index

open Elmish
open Fable.Remoting.Client
open Pages.Todo
open Pages.Login
open Pages.Navigator
open Shared


type MenuModel = { UserName: string }

type CurrentPage =
    | TodoPage
    | LoginPage

type Model =
    { MenuModel: MenuModel
      TodoModel: TodoModel
      LoginModel: LoginModel
      CurrentPage: CurrentPage }



type Msg =
    | TodoMsg of ITodoMsg
    | LoginMsg of LoginMsg
    | NavigatorMsg of NavigatorMsg

let init (): Model * Cmd<Msg> =
    let todoModel = { Todos = []; Input = "" }
    let userName = "hogekun"
    let loginModel = { UserName = userName; Password = "" }

    let cmd =
        Cmd.OfAsync.perform todosApi.getTodos () (TodoMsg << (fun x -> (GotTodo x) :> ITodoMsg))

    let menuModel = { UserName = userName }
    ({ MenuModel = menuModel
       TodoModel = todoModel
       LoginModel = loginModel
       CurrentPage = TodoPage }),
    cmd

let update (msg: Msg) (model1: Model): Model * Cmd<Msg> =
    match msg, model1 with
    | TodoMsg todomsg, { CurrentPage = TodoPage; TodoModel = todomodel } ->
        let (model, cmd) = (todoUpdate todomsg todomodel)
        ({ model1 with TodoModel = model }, Cmd.map TodoMsg cmd)
    | LoginMsg loginmsg, { CurrentPage = LoginPage; LoginModel = loginModel } ->
        let (model, cmd) = (loginUpdate loginmsg loginModel)
        ({ model1 with
               CurrentPage = LoginPage
               LoginModel = model },
         Cmd.map LoginMsg cmd)
    | NavigatorMsg _, model ->
        let (kaihengo, cmd) = loginInit model.MenuModel.UserName
        ({ model with CurrentPage = LoginPage }, cmd)
    | _, model -> (model, Cmd.none)

open Fable.React

let view (model: Model) (dispatch: Msg -> unit) =
    let CurrentPage = model.CurrentPage
    div [] [
        navigatorView ({ NavigatorDispatch = (dispatch << NavigatorMsg) })
        hr []
        div [] [
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
        ]
    ]
