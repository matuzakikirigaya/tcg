module Server.Auth

open Jwt
open Shared.Model.Login
open System
open FSharp.Control.Tasks.ContextInsensitive
open Giraffe
open Saturn.ControllerHelpers
open Microsoft.AspNetCore.Http

let createUser (login: LoginModel): UserData =
    { userName = login.UserName
      token = generateToken login.UserName }

let login (next: HttpFunc) (ctx: HttpContext) =
    task {
        let! login = ctx.BindJsonAsync<LoginModel>()

        return! if login.isValid () then
                    let data = createUser login
                    ctx.WriteJsonAsync data
                else
                    Response.unauthorized ctx "Bearer" "" (sprintf "User '%s' can't be logged in." login.UserName)
    }
