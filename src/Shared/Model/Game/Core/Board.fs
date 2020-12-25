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

let initialServerBoard: ServerBoard =
    { serverPlayer1 = Player1
      serverPlayer2 = Player2
      serverTurn = Standby }
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
