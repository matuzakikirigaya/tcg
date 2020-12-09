module Index

open Elmish
open Fable.Remoting.Client
open Pages.Todo
open Pages.Login
open Shared

type Model =
    | TodoModel of TodoModel
    | LoginModel of LoginModel

type Msg =
    | TodoMsg of TodoMsg
    | LoginMsg of LoginMsg

let init (): Model * Cmd<Msg> =
    let model = { Todos = []; Input = "" }

    let cmd =
        Cmd.OfAsync.perform todosApi.getTodos () (TodoMsg << GotTodos)

    TodoModel model, cmd

let update (msg: Msg) (model: Model): Model * Cmd<Msg> =
    match msg, model with
    | TodoMsg todomsg, TodoModel todomodel ->
        let (model, cmd) = (todoUpdate todomsg todomodel) in (TodoModel model, Cmd.map TodoMsg cmd)
    | LoginMsg loginmsg, LoginModel loginmodel ->
        let (model, cmd) = (loginUpdate loginmsg loginmodel) in (LoginModel model, Cmd.map LoginMsg cmd)
    | _, model -> (model, Cmd.none)

let view (model: Model) (dispatch: Msg -> unit) =
    match model with
    | TodoModel todoModel ->
        todoView
            { TodoModel = todoModel
              TodoDispatch = (dispatch << TodoMsg) }
    | LoginModel loginModel ->
        loginView
            { loginModel = loginModel
              loginDispatch = (dispatch << LoginMsg) }
