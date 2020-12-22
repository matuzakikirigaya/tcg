module Shared.Model.Game.Board

open Shared.Model.Game.Card
open Shared.Model.Game.Turn

type ServerPlayer =
    { serverPlayerVanguard: list<VanguardCard>
      serverPlayerRearguard: list<RearguardCard>
      serverPlayerDeck: list<DeckCard>
      serverPlayerGraveyard: list<GraveyardCard>
      serverPlayerHand: list<HandCard>
      serverPlayerLife: int
      serverPlayerMana: int }

type ClientSelfPlayer =
    { selfVanguard: list<VanguardCard>
      selfRearguard: list<RearguardCard>
      selfDeck: int
      selfGraveyard: list<GraveyardCard>
      selfHand: list<HandCard>
      selfLife: int
      selfMana: int }

type ClientOpponentPlayer =
    { opponentVanguard: list<VanguardCard>
      opponentRearguard: list<RearguardCard>
      opponentDeck: int
      opponentGraveyard: list<GraveyardCard>
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
