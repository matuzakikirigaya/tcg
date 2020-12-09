module Pages.Navigator

open Client.Utils.ElmishView


open Fable.React

type NavigatorMsg = JumpToLogOut

type NavigotorProps = { NavigatorDispatch: Unit -> Unit }

let navigatorView =
    elmishView "navigator" <| fun { NavigatorDispatch = dispatch } ->
        div [] [
            yield str "HOGE"
            yield button [
                       // Button.Disabled (Todo.isValid model.Input |> not)
                       //button.OnClick(fun _ -> dispatch ())
                        ] [
                str "Add"
            ]
        ]
