module Server.WebSocket

open Saturn
open FSharp.Control.Tasks.V2
open Thoth.Json
open Giraffe.Core

open Microsoft.Extensions.Logging
open Shared.Model.WebSocket
open Thoth.Json.Net
open Server.Game.Dummy
open Shared.Model.Game.Board

open Server.Game.Handler.SendChatSubstance
open Server.Game.Handler.SendClientBoard
open Server.Game.Handler.JoinWebSocket
/// Sets up the channel to listen to clients.
let channel =
    channel {
        join joinWebSocket
        handle ClientSourceApi.GetTopicName.SendChatSubstance SendChatSubstanceHandler
        handle ClientSourceApi.GetTopicName.GetGameBoard sendClientBoard
    }
