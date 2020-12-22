module Client.Game.Chat

open Shared.Model.WebSocket

type ChatMsg =
    | MSubmitSubstance
    | MChangeSubstance of ChatSubstance
    | MReceivedSubstance of ChatSubstance

open Elmish
open Fable.React
open Fable.React.Props

type ChatModel =
    { ChatSender: ChatSubstance -> unit
      ReceivedSubstance: list<ChatSubstance>
      SendingSubstance: ChatSubstance
      UserName: string }
    member This.Update msg: ChatModel * Cmd<ChatMsg> =
        match msg with
        | MSubmitSubstance ->
            This.ChatSender This.SendingSubstance
            This, Cmd.none
        | MChangeSubstance sub -> { This with SendingSubstance = sub }, Cmd.none
        | MReceivedSubstance sub ->
            { This with
                  ReceivedSubstance = sub :: This.ReceivedSubstance },
            Cmd.none

    member This.view dispatch =
        div [] [
            input [ OnChange
                        (fun ev ->
                            MChangeSubstance
                                { This.SendingSubstance with
                                      substance = (ev.Value)
                                      userName = This.UserName }
                            |> dispatch) ]
            button [ OnMouseDown(fun ev -> dispatch MSubmitSubstance)
                     ClassName "msr_btn13" ] [
                str "submit"
            ]
            div []
            <| List.map
                (fun value ->
                    div [] [
                        str <| value.userName + ":"
                        br []
                        div [ Class "substance_right" ] [
                            str value.substance
                        ]
                    ])
                This.ReceivedSubstance
        ]
