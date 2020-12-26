module Client.Game.Api.Index

open Browser.WebSocket
open Browser.Types
open Shared.Model.Game.Board
open Shared.Model.WebSocket

let buildClientSender (ws: WebSocket) clientapi =

    match clientapi with
    | SendChatSubstance message ->
        let message =
            {| Topic = ClientSourceApi.GetTopicName.SendChatSubstance
               Ref = ""
               Payload = message |}

        let message =
            Thoth.Json.Encode.Auto.toString (0, message)

        ws.send message
    | GameApi api ->
        match api with
        | GetGameBoard props -> // ここをGameのApiの部分に書き換えたい
            let message =
                {| Topic = ClientSourceApi.GetTopicName.GetGameBoard
                   Ref = ""
                   Payload = props |}

            let message =
                Thoth.Json.Encode.Auto.toString (0, message)

            ws.send message
        | Draw props -> // ここをGameのApiの部分に書き換えたい
            let message =
                {| Topic = ClientSourceApi.GetTopicName.Draw
                   Ref = ""
                   Payload = props |}

            let message =
                Thoth.Json.Encode.Auto.toString (0, message)

            ws.send message

        | DevInit props -> // ここをGameのApiの部分に書き換えたい
            let message =
                {| Topic = ClientSourceApi.GetTopicName.DevInit
                   Ref = ""
                   Payload = props |}

            let message =
                Thoth.Json.Encode.Auto.toString (0, message)

            ws.send message
