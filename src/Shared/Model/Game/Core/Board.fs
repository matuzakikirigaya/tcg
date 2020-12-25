module Shared.Model.Game.Board

open Shared.Model.Game.Card
open Shared.Model.Game.Turn

type ServerPlayer =
    { serverPlayerVanguard: Vanguard
      serverPlayerRearguard: Rearguard
      serverPlayerDeck: Deck
      serverPlayerGraveyard: Graveyard
      serverPlayerHand: Hand
      serverPlayerLife: int
      serverPlayerMana: int
      serverPlayerName: string }

type ClientSelfPlayer =
    { selfVanguard: Vanguard
      selfRearguard: Rearguard
      selfDeck: int
      selfGraveyard: Graveyard
      selfHand: Hand
      selfLife: int
      selfMana: int }

type ClientOpponentPlayer =
    { opponentVanguard: Vanguard
      opponentRearguard: Rearguard
      opponentDeck: int
      opponentGraveyard: Graveyard
      opponentHand: int
      opponentLife: int
      opponentMana: int }

type ServerBoard =
    { serverPlayer1: ServerPlayer
      serverPlayer2: ServerPlayer
      serverTurn: Turn }

type ClientBoard =
    { clientSelfPlayer: ClientSelfPlayer
      clientOpponentPlayer: ClientOpponentPlayer
      clientTurn: Turn }

let chiralBoard (board: ServerBoard): ServerBoard =
    { board with
          serverPlayer1 = board.serverPlayer2
          serverPlayer2 = board.serverPlayer1 }

let chiralBoardOrNot (board: ServerBoard, name: string) =
    if board.serverPlayer1.serverPlayerName = name
    then board, id
    else if board.serverPlayer2.serverPlayerName = name
    then chiralBoard board, chiralBoard
    else raise (System.ArgumentException("Divisor cannot be zero!"))
