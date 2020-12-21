module Pages.Login

open Shared
open Elmish
open Client.Utils.Msg
open Shared.Model.Login

open System
open Thoth.Json
open Fable
open Fetch.Types
open Fable.Core.JsInterop

type LoginMsg =
    | StorageFailure of exn
    | LoggedIn of UserData
    | LoginSuccess of UserData
    | SetUserName of string
    | SetPassword of string
    | AuthError of exn
    | LoginClicked

type LoginModel =
    { login: Login
      ErrorMsg: option<exn>
      IsRunning: bool
      UserData: Never<UserData> }
    member loginModel.Update msg =
        match msg with
        | StorageFailure exn ->
            printfn "failure:%A" exn
            loginModel, Cmd.none
        | LoggedIn userData ->
            { loginModel with
                  UserData = Some userData },
            Cmd.none
        | LoginSuccess userData ->
            loginModel,
            Cmd.OfFunc.either (LocalStorage.save "user") userData (fun _ -> LoggedIn userData) StorageFailure
        | SetUserName userName ->
            { loginModel with
                  login =
                      { loginModel.login with
                            userName = userName } },
            Cmd.none
        | SetPassword password ->
            { loginModel with
                  login =
                      { loginModel.login with
                            password = password } },
            Cmd.none
        | AuthError exn -> { loginModel with ErrorMsg = Some exn }, Cmd.none
        | LoginClicked ->

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

            { loginModel with IsRunning = true },
            Cmd.OfPromise.perform authUser loginModel.login (fun x -> LoginSuccess(x)) //(fun x -> AuthError(x) :> LoginMsg)






open Fable.React
open Fable.React.Props
open Fulma

type LoginProps =
    { loginModel: LoginModel
      loginDispatch: LoginMsg -> unit }


let loginView { loginModel = model
                loginDispatch = dispatch }
              =
    div [] [
        input [ HTMLAttr.Placeholder "username"
                HTMLAttr.DefaultValue model.login.userName
                OnChange(fun ev -> dispatch (SetUserName ev.Value)) ]
        input [ HTMLAttr.Placeholder "passwords"
                HTMLAttr.Type "password"
                OnChange(fun ev -> dispatch (SetPassword ev.Value)) ]
        button [ OnClick(fun _ -> dispatch LoginClicked) ] [
            str "submit"
        ]
        div [] [
            str (
                match model.UserData with
                | Some x -> x.userName
                | _ -> "naiyo"
            )
        ]
    ]
