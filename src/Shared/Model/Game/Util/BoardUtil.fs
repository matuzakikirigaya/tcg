module Shared.Model.Game.Util.BoardUtil

open Shared.Model.Game.Board


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

let covertServerBoardIntoClientBoardByName (playerName: string) (board: ServerBoard): ClientBoard = //ここらへん、ハンドラをeitherで書き直したし
    if playerName = board.serverPlayer1.serverPlayerName then
        { clientSelfPlayer = convertServerPlayerToClientSelfPlayer board.serverPlayer1
          clientOpponentPlayer = convertServerPlayerToClientOpponentPlayer board.serverPlayer2
          clientTurn = board.serverTurn }
    else if playerName = board.serverPlayer2.serverPlayerName then
        { clientSelfPlayer = convertServerPlayerToClientSelfPlayer board.serverPlayer2
          clientOpponentPlayer = convertServerPlayerToClientOpponentPlayer board.serverPlayer1
          clientTurn = board.serverTurn }
    else
        { clientSelfPlayer = convertServerPlayerToClientSelfPlayer board.serverPlayer1
          clientOpponentPlayer = convertServerPlayerToClientOpponentPlayer board.serverPlayer2
          clientTurn = board.serverTurn }

let soroeruServerBoardByName (playerName: string) (board: ServerBoard): ServerBoard = //ここらへん、ハンドラをeitherで書き直したし
    if playerName = board.serverPlayer1.serverPlayerName then
        board
    else if playerName = board.serverPlayer2.serverPlayerName then
        { board with
              serverPlayer1 = board.serverPlayer1
              serverPlayer2 = board.serverPlayer2 }
    else
        board
