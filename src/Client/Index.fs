module Index

open Elmish
open Fable.Remoting.Client
open Pages.Todo
open Pages.Login
open Pages.Navigator
open Shared


type MenuModel = { UserName: string }

type PageModel =
    | TodoModel of TodoModel
    | LoginModel of LoginModel

type Model =
    { MenuModel: MenuModel
      PageModel: PageModel }

type Msg =
    | TodoMsg of TodoMsg
    | LoginMsg of LoginMsg
    | NavigatorMsg of NavigatorMsg

let init (): Model * Cmd<Msg> =
    let model = { Todos = []; Input = "" }

    let cmd =
        Cmd.OfAsync.perform todosApi.getTodos () (TodoMsg << GotTodos)

    let pageModel = TodoModel model
    let menuModel = { UserName = "hogekun" }
    ({ MenuModel = menuModel
       PageModel = pageModel }),
    cmd

let update (msg: Msg) (model1: Model): Model * Cmd<Msg> =
    match msg, model1 with
    | TodoMsg todomsg, { PageModel = TodoModel todomodel } ->
        let (model, cmd) = (todoUpdate todomsg todomodel)
        ({ model1 with
               PageModel = TodoModel model },
         Cmd.map TodoMsg cmd)
    | LoginMsg loginmsg, { PageModel = LoginModel loginmodel } ->
        let (model, cmd) = (loginUpdate loginmsg loginmodel)
        ({ model1 with
               PageModel = LoginModel model },
         Cmd.map LoginMsg cmd)
    | NavigatorMsg navigatorMsg, model ->
        let (kaihengo, cmd) = loginInit model.MenuModel.UserName
        ({ model with
               PageModel = LoginModel kaihengo },
         cmd)
    | _, model -> (model, Cmd.none)

open Fable.React
open Client.Utils.ElmishView

let view (model: Model) (dispatch: Msg -> unit) =
    let pageModel = model.PageModel
    div [] [
        // navigatorView ({ NavigatorDispatch = fun () -> () })
        div [] [
            match pageModel with
            | TodoModel todoModel ->
                    yield todoView
                        { TodoModel = todoModel
                          TodoDispatch = (dispatch << TodoMsg) }
            | LoginModel loginModel ->
                    yield loginView
                        { loginModel = loginModel
                          loginDispatch = (dispatch << LoginMsg) }
        ]
    ]
