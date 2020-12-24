module Shared.Model.Game.Board

open Shared.Model.Game.Card
open Shared.Model.Game.Turn

type ServerPlayer =
    { serverPlayerVanguard: Vanguard
      serverPlayerRearguard: Vanguard
      serverPlayerDeck: Deck
      serverPlayerGraveyard: Graveyard
      serverPlayerHand: Hand
      serverPlayerLife: int
      serverPlayerMana: int }

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
