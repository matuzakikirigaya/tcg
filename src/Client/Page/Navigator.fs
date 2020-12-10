module Pages.Navigator

open Client.Utils.ElmishView


open Fable.React

type NavigatorMsg = JumpToLogOut

type NavigotorProps =
    { NavigatorDispatch: NavigatorMsg -> Unit }

open Fable.React.Props

let navigatorView =
    fun { NavigatorDispatch = dispatch } ->
        div [] [
            yield
                button [ OnClick (fun _ -> dispatch JumpToLogOut) ] [
                    str "Add"
                ]
        ]
