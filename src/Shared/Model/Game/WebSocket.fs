module Shared.Model.WebSocket

(* type WebSocketUnadapted = {
    communicationType:string
} *)
type ChatSubstance = { substance: string; userName: string }

type ClientApi =
    | SendChatSubstance of ChatSubstance
    | GetGameBoard
    static member GetTopicNameWithApi(api: ClientApi) =
        match api with
        | SendChatSubstance _ -> "sendChatSubstance"
        | GetGameBoard -> "getGameBoard"

    static member GetTopicName =
        {| SendChatSubstance = "sendChatSubstance"
           GetGameBoard = "getGameBoard" |}
