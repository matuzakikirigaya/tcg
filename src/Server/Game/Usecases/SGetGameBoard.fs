module Server.Game.Usecases.SGetGameBoard

open Shared.Model.Game.ClientApi.SimplyName
open Shared.Model.Game.Board
open Shared.Model.Game.Card
open Shared.Model.Game.GameElmish
open System


let sGetGameBoard: ServerBoard * sGetGameBoardApi -> ServerBoard * GameCmd<sGetGameBoardApi> =
    fun (board, props) ->
        let board1, undoFun =
            chiralBoardOrNot (board, props.playerName)

        let nextBoard =
            { board1 with
                  serverPlayer1 =
                      { board1.serverPlayer1 with
                            serverPlayerSocketId = Some props.socketId } }

        undoFun nextBoard, []
