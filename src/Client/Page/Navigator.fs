module Pages.Navigator

open Client.Utils.ElmishView
open Client.Utils.Msg
open Elmish

open Fable.React

type CurrentPage =
    | LoginPage
    | TodoPage

type NavigatorModel = { CurrentPage: CurrentPage }
type INavigatorMsg = IMsg<NavigatorModel>

type JumpToLogIn() =
    class
        interface INavigatorMsg with
            member this.Update model =
                { model with CurrentPage = LoginPage }, Cmd.none
    end

type JumpToTodo() =
    class
        interface INavigatorMsg with
            member this.Update model =
                { model with CurrentPage = TodoPage }, Cmd.none
    end

type NavigatorMsgFactory() =
    class
        member _.JumpToLogIn() = JumpToLogIn() :> INavigatorMsg
        member _.JumpToTodo() = JumpToTodo() :> INavigatorMsg
    end

type NavigotorProps =
    { NavigatorDispatch: INavigatorMsg -> Unit }


open Fable.React.Props

let navigatorView =
    fun { NavigatorDispatch = dispatch } ->
        let navigatorMsgFactory = NavigatorMsgFactory()
        let jumpToLogIn = navigatorMsgFactory.JumpToLogIn()
        let jumpToTodo = navigatorMsgFactory.JumpToTodo()
        div [] [
            button [ OnClick(fun _ -> dispatch jumpToTodo) ] [
                str "Todo"
            ]
            button [ OnClick(fun _ -> dispatch jumpToLogIn) ] [
                str "Login"
            ]
        ]
