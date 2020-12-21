module Pages.Navigator

open Client.Utils.ElmishView
open Client.Utils.Msg
open Elmish
open Shared

open Fable.React

type CurrentPage =
    | LoginPage
    | TodoPage
    | WebSocketPage


type NavigatorMsg =
    | JumpToLogIn
    | JumpToTodo
    | JumpToChat

type NavigatorModel =
    { CurrentPage: CurrentPage
      User: string }
    member model.Update msg =
        match msg with
        | JumpToLogIn -> { model with CurrentPage = LoginPage }, Cmd.none

        | JumpToTodo -> { model with CurrentPage = TodoPage }, Cmd.none

        | JumpToChat ->
            { model with
                  CurrentPage = WebSocketPage },
            Cmd.none

type NavigotorProps =
    { NavigatorDispatch: NavigatorMsg -> Unit
      NavigatorModel: NavigatorModel }


open Fable.React.Props

let navigatorView =
    fun { NavigatorDispatch = dispatch
          NavigatorModel = navigatorModel } ->
        let buttonCss = "navigatorList"
        let navigatorContent = "navigatorContent"


        div [] [
            div [] [ str navigatorModel.User ]
            div [ Class buttonCss ] [
                div [ OnClick(fun _ -> dispatch JumpToTodo)
                      Class navigatorContent ] [
                    str "Todo"
                ]

                div [ OnClick(fun _ -> dispatch JumpToLogIn)
                      Class navigatorContent ] [
                    str "Login"
                ]

                div [ OnClick(fun _ -> dispatch JumpToChat)
                      Class navigatorContent ] [
                    str "Chat"
                ]
            ]
        ]
