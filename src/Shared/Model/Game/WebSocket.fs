module Shared.Model.WebSocket

open Shared.Model.Game.Board
open System

(* type WebSocketUnadapted = {
    communicationType:string
} *)
type ChatSubstance = { substance: string; userName: string }

type ClientSourceApi =
    | SendChatSubstance of ChatSubstance
    | GetGameBoard
    static member GetTopicName =
        {| SendChatSubstance = "sendChatSubstance"
           GetGameBoard = "getGameBoard" |}
type ClientSinkApi =
    | ReceivedChatSubstance of ChatSubstance
    | GotGameBoard of ClientBoard
    | Error
    static member GetTopicName =
        {| ReceivedChatSubstance = "receivedChatSubstance"
           GotGameBoard = "gotGameBoard" |}
