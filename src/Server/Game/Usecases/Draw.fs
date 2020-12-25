module Server.Game.Usecases.Draw

open Shared.Model.Game.ClientApi.Draw
open Shared.Model.Game.Board
open Shared.Model.Game.Card
open Shared.Model.Game.GameElmish

let drawUpdate: ServerBoard * DrawProps -> ServerBoard * GameCmd<DrawProps> =
    fun (board, props) ->
        let hoge =
            match board.serverPlayer1.serverPlayerDeck with
            | x :: xs ->
                { board with
                      serverPlayer1 =
                          { board.serverPlayer1 with
                                serverPlayerDeck = xs
                                serverPlayerHand =
                                    (CardModule.DCtoHC x)
                                    :: board.serverPlayer1.serverPlayerHand } }
            | _ -> board

        hoge, []
