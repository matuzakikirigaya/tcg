module Server.WebSocket

open Saturn

open Shared.Model.WebSocket

open Server.Game.Handler.SendChatSubstance
open Server.Game.Handler.SendClientBoard
open Server.Game.Handler.Draw
open Server.Game.Handler.JoinWebSocket
open Server.Game.Handler.DevInit
/// Sets up the channel to listen to clients.
let channel =
    channel {
        join joinWebSocket
        handle ClientSourceApi.GetTopicName.SendChatSubstance SendChatSubstanceHandler
        handle ClientSourceApi.GetTopicName.Draw drawHandler
        handle ClientSourceApi.GetTopicName.GetGameBoard sendClientBoard
        handle ClientSourceApi.GetTopicName.DevInit DevInitHandler
    }
