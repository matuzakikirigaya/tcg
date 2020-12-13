module Pages.Login

open Shared
open Elmish
open Client.Utils.Msg
open Shared.Model.Login


type LoginModel =
    { login: Login
      ErrorMsg: option<exn>
      IsRunning: bool
      UserData: Never<UserData> }

type LoginMsg = IMsg<LoginModel>

let StorageFailure (exn) =
    { new LoginMsg with
        member this.Update loginModel =
            printfn "failure:%A" exn
            loginModel, Cmd.none }

let LoggedIn (userData: UserData) =
    { new LoginMsg with
        member this.Update loginModel =
            { loginModel with
                  UserData = Some userData },
            Cmd.none }

let LoginSuccess (userData: UserData) =
    { new LoginMsg with
        member _.Update loginModel =
            loginModel,
            Cmd.OfFunc.either (LocalStorage.save "user") userData (fun _ -> LoggedIn userData :> IMsg<LoginModel>) (fun x ->
                StorageFailure x :> IMsg<LoginModel>) }

let SetUserName (userName: string) =
    { new LoginMsg with
        member _.Update loginModel =
            { loginModel with
                  login =
                      { loginModel.login with
                            userName = userName } },
            Cmd.none }

let SetPassword (password: string) =
    { new LoginMsg with
        member _.Update loginModel =
            { loginModel with
                  login =
                      { loginModel.login with
                            password = password } },
            Cmd.none }

let AuthError (err: exn) =
    { new LoginMsg with
        member _.Update loginModel =
            { loginModel with ErrorMsg = Some err }, Cmd.none }

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
            with _ -> return! failwithf "Could not authenticate user."
    }

let LoginClicked () =
    { new LoginMsg with
        member _.Update loginModel =
            { loginModel with IsRunning = true },
            Cmd.OfPromise.perform authUser loginModel.login (fun x -> LoginSuccess(x)) } //(fun x -> AuthError(x) :> LoginMsg)


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
        button [ OnClick(fun _ -> dispatch (LoginClicked())) ] [
            str "submit"
        ]
        div [] [
            str
                (match model.UserData with
                 | Some x -> x.userName
                 | _ -> "naiyo")
        ]
    ]
