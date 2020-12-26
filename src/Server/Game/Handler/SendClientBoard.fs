module Server.Game.Handler.SendClientBoardi

open FSharp.Control.Tasks.V2

open Saturn
open Thoth.Json
open Giraffe.Core

open Shared.Model.WebSocket
open Thoth.Json.Net
open Microsoft.AspNetCore.Http
open Channels
open Server.Game.Program
open Shared.Model.Game.ClientApi.SimplyName
open Shared.Model.Game.Util.BoardUtil

open Shared.Model.Game.Board

let sendClientBoard2 (hub: Channels.ISocketHub) (serverBoard: ServerBoard) =
    task {
        let s1 =
            serverBoard.serverPlayer1.serverPlayerSocketId

        let s2 =
            serverBoard.serverPlayer2.serverPlayerSocketId

        match s1, s2 with
        | Some s1, Some s2 ->
            let p1 =
                covertServerBoardIntoClientBoardByName serverBoard.serverPlayer1.serverPlayerName serverBoard

            let p2 =
                covertServerBoardIntoClientBoardByName serverBoard.serverPlayer2.serverPlayerName serverBoard

            let payload1 = Encode.Auto.toString (0, p1)
            let payload2 = Encode.Auto.toString (0, p2)
            do! hub.SendMessageToClient "/channel" s1 ClientSinkApi.GetTopicName.GotGameBoard payload1
            do! hub.SendMessageToClient "/channel" s2 ClientSinkApi.GetTopicName.GotGameBoard payload2
        | Some s1, _ ->
            let p1 =
                covertServerBoardIntoClientBoardByName serverBoard.serverPlayer1.serverPlayerName serverBoard
            let payload1 = Encode.Auto.toString (0, p1)
            do! hub.SendMessageToClient "/channel" s1 ClientSinkApi.GetTopicName.GotGameBoard payload1
        | _, Some s2 ->
            let p2 =
                covertServerBoardIntoClientBoardByName serverBoard.serverPlayer2.serverPlayerName serverBoard
            let payload1 = Encode.Auto.toString (0, p2)
            do! hub.SendMessageToClient "/channel" s2 ClientSinkApi.GetTopicName.GotGameBoard payload1
        | _ -> return ()
    }
