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
open Fulma

let navBrand =
    Navbar.Brand.div [ ] [
        Navbar.Item.a [
            Navbar.Item.Props [ Href "https://safe-stack.github.io/" ]
            Navbar.Item.IsActive true
        ] [
            img [
                Src "/favicon.png"
                Alt "Logo"
            ]
        ]
    ]

let containerBox (model : TodoModel) (dispatch : TodoMsg -> unit) =
    Box.box' [ ] [
        Content.content [ ] [
            Content.Ol.ol [ ] [
                for todo in model.Todos do
                    li [ ] [ str todo.Description ]
            ]
        ]
        Field.div [ Field.IsGrouped ] [
            Control.p [ Control.IsExpanded ] [
                Input.text [
                  Input.Value model.Input
                  Input.Placeholder "What needs to be done?"
                  Input.OnChange (fun x -> SetInput x.Value |> dispatch) ]
            ]
            Control.p [ ] [
                Button.a [
                    Button.Color IsPrimary
                    Button.Disabled (Todo.isValid model.Input |> not)
                    Button.OnClick (fun _ -> dispatch AddTodo)
                ] [
                    str "Add"
                ]
            ]
        ]
    ]

type TodoProps = {TodoModel: TodoModel; TodoDispatch: TodoMsg -> Unit }

open Client.Utils.ElmishView

let todoView  = elmishView "todo" ( fun {TodoModel = todoModel; TodoDispatch = todoDispatch} ->
    // Hero.hero [
    //     Hero.Color IsPrimary
    //     Hero.IsFullHeight
    //     Hero.Props [
    //         Style [
    //             Background """linear-gradient(rgba(0, 0, 0, 0.5), rgba(0, 0, 0, 0.5)), url("https://unsplash.it/1200/900?random") no-repeat center center fixed"""
    //             BackgroundSize "cover"
    //         ]
    //     ]
    // ] [
    //     Hero.head [ ] [
    //         Navbar.navbar [ ] [
    //             Container.container [ ] [ navBrand ]
    //         ]
    //     ]

        div [ ] [
            div [ ] [
                div [
                    // Column.Width (Screen.All, Column.Is6)
                    // Column.Offset (Screen.All, Column.Is3)
                ] [
                    Heading.p [ Heading.Modifiers [ Modifier.TextAlignment (Screen.All, TextAlignment.Centered) ] ] [ str "SAFE2" ]
                    containerBox todoModel todoDispatch
                ]
            ]
        ]
    // ]
)