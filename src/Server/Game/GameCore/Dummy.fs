module Server.Game.Dummy

open Shared.Model.Game.Card
open Shared.Model.Game.Board
open Shared.Model.Game.Turn

let gobruiInHand i: HandCard =
    { name = "gobrui"
      id = i
      cardType =
          VanguardType
              { VanguardAttackPoint = 1
                VanguardDefencePoint = 1 } }

let marutaOnVanguard i: VanguardCard = { name = "maruta"; id = i }
let gargoiruOnRearguard i: RearguardCard = { name = "gagoiru"; id = i }

let godTakeshiInDeck i: DeckCard =
    { name = "takeshi"
      id = i
      cardType =
          VanguardType
              { VanguardAttackPoint = 1
                VanguardDefencePoint = 1 } }

let datenshiInGraveyard i: GraveyardCard =
    { name = "datenshi"
      id = i
      cardType =
          VanguardType
              { VanguardAttackPoint = 1
                VanguardDefencePoint = 1 } }

let michaelInGraveyard i: GraveyardCard =
    { name = "michael"
      id = i
      cardType =
          VanguardType
              { VanguardAttackPoint = 1
                VanguardDefencePoint = 1 } }

let Player1: ServerPlayer =
    { serverPlayerVanguard =
          [ marutaOnVanguard 1
            marutaOnVanguard 2 ]
      serverPlayerRearguard =
          [ gargoiruOnRearguard 1
            gargoiruOnRearguard 2
            gargoiruOnRearguard 3
            gargoiruOnRearguard 4
            gargoiruOnRearguard 5 ]
      serverPlayerGraveyard =
          [ michaelInGraveyard 1
            michaelInGraveyard 2
            michaelInGraveyard 3 ]
      serverPlayerDeck =
          [ godTakeshiInDeck 1
            godTakeshiInDeck 2 ]
      serverPlayerHand =
          [ gobruiInHand 1
            gobruiInHand 2
            gobruiInHand 3
            gobruiInHand 4
            gobruiInHand 5
            gobruiInHand 6 ]
      serverPlayerLife = 30
      serverPlayerMana = 0
      serverPlayerName = "たけし" }

let Player2: ServerPlayer =
    { serverPlayerVanguard =
          [ marutaOnVanguard 1
            marutaOnVanguard 2 ]
      serverPlayerRearguard =
          [ gargoiruOnRearguard 1
            gargoiruOnRearguard 2
            gargoiruOnRearguard 3
            gargoiruOnRearguard 3
            gargoiruOnRearguard 3
            gargoiruOnRearguard 3
            gargoiruOnRearguard 3
            gargoiruOnRearguard 3
            gargoiruOnRearguard 4
            gargoiruOnRearguard 5 ]
      serverPlayerGraveyard =
          [ michaelInGraveyard 1
            michaelInGraveyard 2
            michaelInGraveyard 2
            michaelInGraveyard 2
            michaelInGraveyard 2
            michaelInGraveyard 3 ]
      serverPlayerDeck =
          [ godTakeshiInDeck 1
            godTakeshiInDeck 2 ]
      serverPlayerHand =
          [ gobruiInHand 1
            gobruiInHand 2
            gobruiInHand 3
            gobruiInHand 4
            gobruiInHand 5
            gobruiInHand 6 ]
      serverPlayerLife = 30
      serverPlayerMana = 0
      serverPlayerName = "キリト" }

let convertServerPlayerToClientSelfPlayer (serverPlayer: ServerPlayer): ClientSelfPlayer =
    { selfDeck = List.length serverPlayer.serverPlayerDeck
      selfMana = serverPlayer.serverPlayerMana
      selfHand = serverPlayer.serverPlayerHand
      selfVanguard = serverPlayer.serverPlayerVanguard
      selfRearguard = serverPlayer.serverPlayerRearguard
      selfGraveyard = serverPlayer.serverPlayerGraveyard
      selfLife = serverPlayer.serverPlayerLife }

let convertServerPlayerToClientOpponentPlayer (serverPlayer: ServerPlayer): ClientOpponentPlayer =
    { opponentDeck = List.length serverPlayer.serverPlayerDeck
      opponentMana = serverPlayer.serverPlayerMana
      opponentHand = List.length serverPlayer.serverPlayerHand
      opponentVanguard = serverPlayer.serverPlayerVanguard
      opponentRearguard = serverPlayer.serverPlayerRearguard
      opponentGraveyard = serverPlayer.serverPlayerGraveyard
      opponentLife = serverPlayer.serverPlayerLife }

let covertServerBoardIntoClientBoardFor1 (board: ServerBoard): ClientBoard =
    { clientSelfPlayer = convertServerPlayerToClientSelfPlayer board.serverPlayer1
      clientOpponentPlayer = convertServerPlayerToClientOpponentPlayer board.serverPlayer2
      clientTurn = board.serverTurn }

let covertServerBoardIntoClientBoardFor2 (board: ServerBoard): ClientBoard =
    { clientSelfPlayer = convertServerPlayerToClientSelfPlayer board.serverPlayer2
      clientOpponentPlayer = convertServerPlayerToClientOpponentPlayer board.serverPlayer1
      clientTurn = board.serverTurn }

let initialServerBoard: ServerBoard =
    { serverPlayer1 = Player1
      serverPlayer2 = Player2
      serverTurn = Standby }
