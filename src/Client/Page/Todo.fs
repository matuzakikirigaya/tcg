module Pages.Todo

open Shared
open Elmish
open Fable.Remoting.Client
open Fulma
open Client.Utils.ElmishView
open Client.Utils.Msg


type TodoMsg =
    | GotTodo of list<Todo>
    | SetInput of string
    | AddedTodo of Todo
    | AddTodo

let todosApi =
    Remoting.createApi ()
    |> Remoting.withRouteBuilder Route.builder
    |> Remoting.buildProxy<ITodosApi>

type TodoModel =
    { Todos: Todo list
      Input: string }
    member todoModel.Update msg =
        match msg with
        | GotTodo todos -> { todoModel with Todos = todos }, Cmd.none
        | SetInput setInput -> { todoModel with Input = setInput }, Cmd.none
        | AddedTodo todo ->
            { todoModel with
                  Todos = todoModel.Todos @ [ todo ] },
            Cmd.none
        | AddTodo ->
            let todo = Todo.create todoModel.Input

            let cmd: Cmd<TodoMsg> =
                Cmd.OfAsync.perform todosApi.addTodo todo (fun todo -> AddedTodo(todo))

            { todoModel with Input = "" }, cmd



let todoUpdate (msg: TodoMsg) (todoModel: TodoModel): TodoModel * Cmd<TodoMsg> = todoModel.Update msg
open Fable.React
open Fable.React.Props

type TodoProps =
    { TodoModel: TodoModel
      TodoDispatch: TodoMsg -> Unit }

let containerBox { TodoModel = todoModel; TodoDispatch = todoDispatch } =
    div [] [
        div [] [
            ol [] [
                for todo in todoModel.Todos do
                    li [] [ str todo.Description ]
            ]
        ]
        div [] [
            div [] [
                input [ Value todoModel.Input
                        Placeholder "What needs to be done?"
                        OnChange(fun x -> SetInput x.Value |> todoDispatch) ]
            ]
            div [] [
                button [ Disabled(Todo.isValid todoModel.Input |> not)
                         OnClick(fun _ -> AddTodo |> todoDispatch) ] [
                    str "Add"
                ]
            ]
        ]
    ]



let todoView todoProps =
    div [] [
        div [] [
            div [] [ yield containerBox todoProps ]
        ]
    ]
