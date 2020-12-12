module Pages.Login

open Shared
open Elmish
open Client.Utils.Msg


type LoginModel = { UserName: string; Password: string }

type LoginMsg = IMsg<LoginModel>


let loginUpdate (msg: LoginMsg) (loginModel: LoginModel) = loginModel, Cmd.none

let loginInit userName =
    { UserName = userName; Password = "" }, Cmd.none

open Fable.React
open Fable.React.Props
open Fulma

type LoginProps =
    { loginModel: LoginModel
      loginDispatch: LoginMsg -> unit }


let loginView { loginModel = model; loginDispatch = dispatch } = div [] [ yield str model.UserName ]
