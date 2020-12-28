module Server.Game.Handler.SendClientBoardi

open FSharp.Control.Tasks.V2
open Saturn
open System
open Shared.Model.WebSocket
open Thoth.Json.Net
open Shared.Model.Game.Util.BoardUtil
open Shared.Model.Game.Board


let sendClientBoard2 (hub: Channels.ISocketHub) (serverBoard: ServerBoard) =
    task {

        let sendOrNothing (socket: option<Guid>) =
            task {
                match socket with
                | Some socket ->
                    let p1 =
                        covertServerBoardIntoClientBoardByName serverBoard.serverPlayer1.serverPlayerName serverBoard

                    let payload1 = Encode.Auto.toString (0, p1)
                    do! hub.SendMessageToClient "/channel" socket ClientSinkApi.GetTopicName.GotGameBoard payload1
                | _ -> return ()
            }

        do! sendOrNothing serverBoard.serverPlayer1.serverPlayerSocketId
        do! sendOrNothing serverBoard.serverPlayer2.serverPlayerSocketId
    }
