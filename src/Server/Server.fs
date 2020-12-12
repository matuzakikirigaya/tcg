module Server.Program

open Fable.Remoting.Server
open Fable.Remoting.Giraffe
open Saturn

open Shared

type Storage () =
    let todos = ResizeArray<_>()

    member __.GetTodos () =
        List.ofSeq todos

    member __.AddTodo (todo: Todo) =
        if Todo.isValid todo.Description then
            todos.Add todo
            Ok ()
        else Error "Invalid todo"

type LoginStorage () =
    let testUserName = "testuser"
    let testUserPassword = "testPassword"
    let mutable isLoggingIn = false
    member this.loggingIn (login:Login) = if login.userName = testUserName && login.password = testUserPassword
                                                    then isLoggingIn <- true
                                                         true
                                                    else false

let loginStorage = LoginStorage ()

let storage = Storage()

storage.AddTodo(Todo.create "Create new SAFE project") |> ignore
storage.AddTodo(Todo.create "Write your app") |> ignore
storage.AddTodo(Todo.create "Ship it !!!") |> ignore

let todosApi =
    { getTodos = fun () -> async { return storage.GetTodos() }
      addTodo =
        fun todo -> async {
            match storage.AddTodo todo with
            | Ok () -> return todo
            | Error e -> return failwith e
        } }

let loginApi =
    {
        createUser = fun () -> async { return true }
        loggingIn = fun Login -> async { return loginStorage.loggingIn Login }
    }

let webApp page =
    Remoting.createApi()
    |> Remoting.withRouteBuilder Route.builder
    |> Remoting.fromValue page
    |> Remoting.buildHttpHandler

let webApp2 = router {
    forward "" (webApp todosApi)
    get "/login" (webApp loginApi)
    post "/api/users/login/" Auth.login
}

let app =
    application {
        url "http://0.0.0.0:8085"
        use_router webApp2
        memory_cache
        use_jwt_authentication Jwt.secret Jwt.issuer
        use_static "public"
        use_gzip
    }

run app
