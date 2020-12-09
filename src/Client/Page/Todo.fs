module Pages.Todo

open Shared
open Elmish
open Fable.Remoting.Client
open Fulma

type TodoModel =
    { Todos: Todo list
      Input: string }

type TodoMsg =
    | GotTodos of Todo list
    | SetInput of string
    | AddTodo
    | AddedTodo of Todo

let todosApi =
    Remoting.createApi()
    |> Remoting.withRouteBuilder Route.builder
    |> Remoting.buildProxy<ITodosApi>

let todoUpdate (msg: TodoMsg) (todoModel: TodoModel): TodoModel * Cmd<TodoMsg> =
    match msg with
    | GotTodos todos->
        { todoModel with Todos = todos }, Cmd.none
    | SetInput value->
        { todoModel with Input = value }, Cmd.none
    | AddTodo->
        let todo = Todo.create todoModel.Input
        let cmd = Cmd.OfAsync.perform todosApi.addTodo todo AddedTodo
        { todoModel with Input = "" }, cmd
    | AddedTodo todo->
        { todoModel with Todos = todoModel.Todos @ [ todo ] }, Cmd.none
open Fable.React
open Fable.React.Props


let containerBox (model : TodoModel) (dispatch : TodoMsg -> unit) =
    div [ ] [
        div [ ] [
            ol [ ] [
                for todo in model.Todos do
                    li [ ] [ str todo.Description ]
            ]
        ]
        div [] [
            div [ ] [
                input [
                  Value model.Input
                  Placeholder "What needs to be done?"
                  OnChange (fun x -> SetInput x.Value |> dispatch) ]
            ]
            div [ ] [
                button[
                    Disabled (Todo.isValid model.Input |> not)
                    OnClick (fun _ -> dispatch AddTodo)
                ] [
                    str "Add"
                ]
            ]
        ]
    ]

type TodoProps = {TodoModel: TodoModel; TodoDispatch: TodoMsg -> Unit }

open Client.Utils.ElmishView

let todoView  = elmishView "todo" ( fun {TodoModel = todoModel; TodoDispatch = todoDispatch} ->
        div [ ] [
            div [ ] [
                div [
                ] [
                    containerBox todoModel todoDispatch
                ]
            ]
        ]
)