module Pages.Login

open Shared
open Elmish


type LoginModel = { UserName: string; Password: string }

type LoginMsg =
    | LoginSuccess of string
    | SetUserName of string
    | SetPassword of string
    | AuthError of exn
    | LogInClicked

let loginUpdate (msg: LoginMsg) (loginModel: LoginModel) = loginModel, Cmd.none
open Fable.React
open Fable.React.Props
open Fulma

type LoginProps =
    { loginModel: LoginModel
      loginDispatch: LoginMsg -> unit }

open Client.Utils.ElmishView

let loginView  = elmishView "login" (fun { loginModel = model; loginDispatch = dispatch } ->
    Hero.hero [ Hero.Color IsPrimary
                Hero.IsFullHeight
                Hero.Props [ Style [ Background
                                         """linear-gradient(rgba(0, 0, 0, 0.5), rgba(0, 0, 0, 0.5)), url("https://unsplash.it/1200/900?random") no-repeat center center fixed"""
                                     BackgroundSize "cover" ] ] ] [
        Hero.head [] []

        Hero.body [] []
    ]
)