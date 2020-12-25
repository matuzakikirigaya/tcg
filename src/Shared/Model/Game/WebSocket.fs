module Shared.Model.WebSocket

open Shared.Model.Game.Board
open System

open Shared.Model.Game.ClientApi.Draw
(* type WebSocketUnadapted = {
    communicationType:string
} *)
type ChatSubstance = { substance: string; userName: string }

type GameApi = GetGameBoard | Draw of DrawProps | DevInit
type ClientSourceApi =
    | SendChatSubstance of ChatSubstance
    | GameApi of GameApi
    static member GetTopicName =
        {| SendChatSubstance = "sendChatSubstance"
           Draw = "draw"
           GetGameBoard = "getGameBoard"
           DevInit = "devInit" |}

type ClientSinkApi =
    | ReceivedChatSubstance of ChatSubstance
    | GotGameBoard of ClientBoard
    | Error
    static member GetTopicName =
        {| ReceivedChatSubstance = "receivedChatSubstance"
           GotGameBoard = "gotGameBoard" |}
