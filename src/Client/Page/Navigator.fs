module Pages.Navigator

open Client.Utils.ElmishView
open Client.Utils.Msg
open Elmish
open Shared

open Fable.React

type CurrentPage =
    | LoginPage
    | TodoPage

type NavigatorModel =
    { CurrentPage: CurrentPage
      User: string }

type INavigatorMsg = IMsg<NavigatorModel>

let JumpToLogIn =
    { new INavigatorMsg with
        member this.Update model =
            { model with CurrentPage = LoginPage }, Cmd.none }

let JumpToTodo =
    { new INavigatorMsg with
        member this.Update model =
            { model with CurrentPage = TodoPage }, Cmd.none }

type NavigotorProps =
    { NavigatorDispatch: INavigatorMsg -> Unit
      NavigatorModel: NavigatorModel }


open Fable.React.Props

let navigatorView =
    fun { NavigatorDispatch = dispatch; NavigatorModel = navigatorModel } ->
        div [] [
            button [ OnClick(fun _ -> dispatch JumpToTodo) ] [
                str "Todo"
            ]
            button [ OnClick(fun _ -> dispatch JumpToLogIn) ] [
                str "Login"
            ]
            div [] [ str navigatorModel.User ]
        ]
