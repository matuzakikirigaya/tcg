module Pages.Todo

open Shared
open Elmish
open Fable.Remoting.Client
open Fulma
open Client.Utils.ElmishView

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

type TodoProps = {TodoModel: TodoModel; TodoDispatch: TodoMsg -> Unit }

let containerBox {TodoModel = todoModel; TodoDispatch = todoDispatch} =
    div [ ] [
        div [ ] [
            ol [ ] [
                for todo in todoModel.Todos do
                    li [ ] [ str todo.Description ]
            ]
        ]
        div [] [
            div [ ] [
                input [
                  Value todoModel.Input
                  Placeholder "What needs to be done?"
                  OnChange (fun x -> SetInput x.Value |> todoDispatch) ]
            ]
            div [ ] [
                button[
                    Disabled (Todo.isValid todoModel.Input |> not)
                    OnClick (fun _ -> todoDispatch AddTodo)
                ] [
                    str "Add"
                ]
            ]
        ]
    ]



let todoView  todoProps =
        div [ ] [
            div [ ] [
                div [
                ] [
                    yield containerBox todoProps
                ]
            ]
        ]