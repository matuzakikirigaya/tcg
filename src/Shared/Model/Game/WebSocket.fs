module Shared.Model.WebSocket

(* type WebSocketUnadapted = {
    communicationType:string
} *)
type ChatSubstance = { substance: string; userName: string }

type ClientSourceApi =
    | SendChatSubstance of ChatSubstance
    | GetGameBoard
    static member GetTopicNameWithApi(api: ClientSourceApi) =
        match api with
        | SendChatSubstance _ -> "sendChatSubstance"
        | GetGameBoard -> "getGameBoard"
    static member GetTopicName =
        {| SendChatSubstance = "sendChatSubstance"
           GetGameBoard = "getGameBoard" |}

// type 