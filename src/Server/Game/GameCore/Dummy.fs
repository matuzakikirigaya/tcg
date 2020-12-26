module Server.Game.Dummy

open Shared.Model.Game.Card
open Shared.Model.Game.Board
open Shared.Model.Game.Turn


let convertServerPlayerToClientSelfPlayer (serverPlayer: ServerPlayer): ClientSelfPlayer =
    { selfDeck = List.length serverPlayer.serverPlayerDeck
      selfMana = serverPlayer.serverPlayerMana
      selfHand = serverPlayer.serverPlayerHand
      selfVanguard = serverPlayer.serverPlayerVanguard
      selfRearguard = serverPlayer.serverPlayerRearguard
      selfGraveyard = serverPlayer.serverPlayerGraveyard
      selfLife = serverPlayer.serverPlayerLife
      selfName = serverPlayer.serverPlayerName }

let convertServerPlayerToClientOpponentPlayer (serverPlayer: ServerPlayer): ClientOpponentPlayer =
    { opponentDeck = List.length serverPlayer.serverPlayerDeck
      opponentMana = serverPlayer.serverPlayerMana
      opponentHand = List.length serverPlayer.serverPlayerHand
      opponentVanguard = serverPlayer.serverPlayerVanguard
      opponentRearguard = serverPlayer.serverPlayerRearguard
      opponentGraveyard = serverPlayer.serverPlayerGraveyard
      opponentLife = serverPlayer.serverPlayerLife
      opponentName = serverPlayer.serverPlayerName }

let covertServerBoardIntoClientBoardFor1 (board: ServerBoard): ClientBoard =
    { clientSelfPlayer = convertServerPlayerToClientSelfPlayer board.serverPlayer1
      clientOpponentPlayer = convertServerPlayerToClientOpponentPlayer board.serverPlayer2
      clientTurn = board.serverTurn }

let covertServerBoardIntoClientBoardFor2 (board: ServerBoard): ClientBoard =
    { clientSelfPlayer = convertServerPlayerToClientSelfPlayer board.serverPlayer2
      clientOpponentPlayer = convertServerPlayerToClientOpponentPlayer board.serverPlayer1
      clientTurn = board.serverTurn }

