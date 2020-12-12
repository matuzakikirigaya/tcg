module Pages.Todo

open Shared
open Elmish
open Fable.Remoting.Client
open Fulma
open Client.Utils.ElmishView
open Client.Utils.Msg

type TodoModel = { Todos: Todo list; Input: string }

type ITodoMsg = IMsg<TodoModel>

type private GotTodo(todos: list<Todo>) =
    class
        let todos = todos

        interface ITodoMsg with
            member this.Update todoModel =
                { todoModel with Todos = todos }, Cmd.none
    end

type private SetInput(setInput) =
    class
        let setInput = setInput

        interface ITodoMsg with
            member this.Update todoModel =
                { todoModel with Input = setInput }, Cmd.none
    end

type private AddedTodo(todo) =
    class
        let todo = todo

        interface ITodoMsg with
            member this.Update todoModel =
                let model =
                    { todoModel with
                          Todos = todoModel.Todos @ [ todo ] }

                model, Cmd.none
    end

let todosApi =
    Remoting.createApi ()
    |> Remoting.withRouteBuilder Route.builder
    |> Remoting.buildProxy<ITodosApi>

type private AddTodo() =
    class
        interface ITodoMsg with
            member this.Update todoModel =
                let todo = Todo.create todoModel.Input

                let cmd =
                    Cmd.OfAsync.perform todosApi.addTodo todo (fun todo -> (new AddedTodo(todo)) :> ITodoMsg)

                { todoModel with Input = "" }, cmd
    end

type ITodoMsgFactory() =
    class
        member _.AddTodo() = (AddTodo()) :> ITodoMsg
        member _.AddedTodo(todo) = (AddedTodo(todo)) :> ITodoMsg
        member _.SetInput(setInput) = (SetInput(setInput)) :> ITodoMsg
        member _.GotTodo(todos: list<Todo>) = (GotTodo(todos: list<Todo>)) :> ITodoMsg
    end

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
