module Server.Game.Usecases.Draw

open Shared.Model.Game.ClientApi.Draw
open Shared.Model.Game.Board
open Shared.Model.Game.Card
open Shared.Model.Game.GameElmish

let drawUpdate: ServerBoard * DrawProps -> ServerBoard * GameCmd<DrawProps> =
    fun (board, props) ->
        let board1, undoFun =
            chiralBoardOrNot (board, props.playerName)

        let nextBoard =
            match board1.serverPlayer1.serverPlayerDeck with
            | x :: xs ->
                { board1 with
                      serverPlayer1 =
                          { board1.serverPlayer1 with
                                serverPlayerDeck = xs
                                serverPlayerHand =
                                    (CardModule.DCtoHC x)
                                    :: board1.serverPlayer1.serverPlayerHand } }
            | _ -> board1

        undoFun nextBoard, []
