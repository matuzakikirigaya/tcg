module Server.Dummy

open Fable.Remoting.Server
open Fable.Remoting.Giraffe
open Saturn
open Server.Todo

open Shared

let webApp page =
    Remoting.createApi()
    |> Remoting.withRouteBuilder Route.builder
    |> Remoting.fromValue page
    |> Remoting.buildHttpHandler

let webApp2 = router {
    forward "" (webApp todosApi)
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
        add_channel "/channel" Server.WebSocket.channel
    }

run app
