module Pages.Login

open Shared
open Elmish
open Client.Utils.Msg
open Shared.Model.Login


type LoginModel =
    { login: Login
      ErrorMsg: option<exn>
      IsRunning: bool
      UserData: option<UserData> }

type LoginMsg = IMsg<LoginModel>

type LoginSuccess(userData: UserData) =
    class
        interface LoginMsg with
            member _.Update loginModel =
                { loginModel with
                      UserData = Some userData },
                Cmd.none
    end

type SetUserName(userName: string) =
    class
        interface LoginMsg with
            member _.Update loginModel =
                { loginModel with
                      login =
                          { loginModel.login with
                                userName = userName } },
                Cmd.none
    end

type SetPassword(password: string) =
    class
        interface LoginMsg with
            member _.Update loginModel =
                { loginModel with
                      login =
                          { loginModel.login with
                                password = password } },
                Cmd.none
    end

type AuthError(err: exn) =
    class
        interface LoginMsg with
            member _.Update loginModel =
                { loginModel with ErrorMsg = Some err }, Cmd.none
    end

open System
open Thoth.Json
open Fable
open Fetch.Types
open Fable.Core.JsInterop

let authUser (login: Login) =
    promise {
        if String.IsNullOrEmpty login.userName then
            return! failwithf "You need to fill in a username."
        else if String.IsNullOrEmpty login.password then
            return! failwithf "You need to fill in a password."
        else

            let body = Encode.Auto.toString (0, login)

            let props =
                [ Method HttpMethod.POST
                  Fetch.requestHeaders [ ContentType "application/json" ]
                  Body !^body ]

            try
                let! res = Fetch.fetch "/api/users/login/" props
                let! txt = res.text ()
                return Decode.Auto.unsafeFromString<UserData> txt
                // return {UserName = txt; Token = txt}
                // return Decode.Auto.unsafeFromString<UserData> "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJmYXNkZnNkIiwianRpIjoiNjFjN2M3OTctNzhiYy00YzlhLTgwM2MtYjBmMzI5YjIxMGQ2IiwibmJmIjoxNjA3Nzk4NDUxLCJleHAiOjE2MDc4MDIwNTEsImlzcyI6InNhZmVib29rc3RvcmUuaW8iLCJhdWQiOiJzYWZlYm9va3N0b3JlLmlvIn0.4pMuJPbq4yP7Naot7r3Q6kASFWsce2g_9qFFRZue4Gc"
            with _ -> return! failwithf "Could not authenticate user."
    }

type LoginClicked() =
    class
        interface LoginMsg with
            member _.Update loginModel =
                { loginModel with IsRunning = true },
                Cmd.OfPromise.perform authUser loginModel.login (fun x -> LoginSuccess(x) :> LoginMsg) //(fun x -> AuthError(x) :> LoginMsg)
    end


open Fable.React
open Fable.React.Props
open Fulma

type LoginProps =
    { loginModel: LoginModel
      loginDispatch: LoginMsg -> unit }


let loginView { loginModel = model; loginDispatch = dispatch } =
    div [] [
        input [ HTMLAttr.Placeholder "username"
                HTMLAttr.DefaultValue model.login.userName
                OnChange(fun ev -> dispatch (SetUserName ev.Value)) ]
        input [ HTMLAttr.Placeholder "passwords"
                HTMLAttr.Type "password"
                OnChange(fun ev -> dispatch (SetPassword ev.Value)) ]
        button [ OnClick(fun _ -> dispatch (LoginClicked() :> LoginMsg)) ] [
            str "submit"
        ]
        div [] [
            str
                (match model.UserData with
                 | Some x -> x.userName
                 | _ -> "naiyo")
        ]
    ]
