module Pages.Todo

open Shared
open Elmish
open Fable.Remoting.Client
open Fulma
open Client.Utils.ElmishView
open Client.Utils.Msg

type TodoModel = { Todos: Todo list; Input: string }

type ITodoMsg = IMsg<TodoModel>

let GotTodo (todos: list<Todo>) =
    { new ITodoMsg with
        member this.Update todoModel =
            { todoModel with Todos = todos }, Cmd.none }

let SetInput (setInput) =
    { new ITodoMsg with
        member this.Update todoModel =
            { todoModel with Input = setInput }, Cmd.none }

let AddedTodo (todo) =
    { new ITodoMsg with
        member this.Update todoModel =
            { todoModel with
                  Todos = todoModel.Todos @ [ todo ] },
            Cmd.none }


let todosApi =
    Remoting.createApi ()
    |> Remoting.withRouteBuilder Route.builder
    |> Remoting.buildProxy<ITodosApi>

let AddTodo () =
    { new ITodoMsg with
        member this.Update todoModel =
            let todo = Todo.create todoModel.Input

            let cmd: Cmd<ITodoMsg> =
                Cmd.OfAsync.perform todosApi.addTodo todo (fun todo -> AddedTodo(todo))

            { todoModel with Input = "" }, cmd }

let todoUpdate (msg: ITodoMsg) (todoModel: TodoModel): TodoModel * Cmd<ITodoMsg> = msg.Update todoModel
open Fable.React
open Fable.React.Props

type TodoProps =
    { TodoModel: TodoModel
      TodoDispatch: ITodoMsg -> Unit }

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
                         OnClick(fun _ -> todoDispatch <| AddTodo()) ] [
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
