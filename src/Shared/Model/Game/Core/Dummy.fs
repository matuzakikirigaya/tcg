module Shared.Model.Game.Dummy

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
      serverPlayerLife = 60
      serverPlayerMana = 30
      serverPlayerName = "たけし"
      serverPlayerSocketId = None }

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
      serverPlayerLife = 50
      serverPlayerMana = 20
      serverPlayerName = "キリト"
      serverPlayerSocketId = None }

let initialServerBoard: ServerBoard =
    { serverPlayer1 = Player1
      serverPlayer2 = Player2
      serverTurn = Standby }
