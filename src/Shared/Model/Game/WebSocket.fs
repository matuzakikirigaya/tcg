module Shared.Model.WebSocket

(* type WebSocketUnadapted = {
    communicationType:string
} *)
type ChatSubstance = { substance: string; userName: string }

type ClientApi = SendChatSubstance of ChatSubstance
