module Pages.Chat

open Shared.Model.Chat
open Client.Utils.Msg
open Elmish

type ClientChatModel =
    { inputSubmission: SubmissionFromClient
      sentences: ChatSentence list }

type ClientChatMsg = IMsg<ClientChatModel>

let clientMsgBuilder =
    {| submitClicked =
           { new ClientChatMsg with
               member this.Update model = model, Cmd.none }
       addSentence =
           fun sentence ->
               { new ClientChatMsg with
                   member this.Update model =
                       { model with
                             sentences = sentence :: model.sentences },
                       Cmd.none }
       inputSubmissionChanged =
           fun submission ->
               { new ClientChatMsg with
                   member this.Update model =
                       { model with
                             inputSubmission = { submittingChatSentence = submission } },
                       Cmd.none } |}


open Fable.React
open Fable.React.Props

let chatInit () =
    { inputSubmission = { submittingChatSentence = { sentence = "" } }
      sentences = List.Empty },
    Cmd.none

let chatView (props: {| clientChatModel: ClientChatModel
                        dispatch: Dispatch<ClientChatMsg> |}) =
    div [] [
        div [] [
            div [] (List.map (fun x -> str x.sentence) props.clientChatModel.sentences)
        ]
        input [ OnChange(fun ev ->
                    props.dispatch
                    <| clientMsgBuilder.inputSubmissionChanged { sentence = ev.Value }) ]
        button [ OnClick(fun _ -> props.dispatch <| clientMsgBuilder.submitClicked) ] [
            str "submit"
        ]
    ]
