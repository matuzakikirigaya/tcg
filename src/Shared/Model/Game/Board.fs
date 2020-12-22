module Shared.Model.Game.Board

open Shared.Model.Game.Card
open Shared.Model.Game.Turn

type ServerPlayer =
    { vanguard: list<VanguardCard>
      rearguard: list<RearguardCard>
      deck: list<DeckCard>
      graveyard: list<GraveyardCard>
      hand: list<HandCard>
      life: int
      mana: int }

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
    { player1: ServerPlayer
      player2: ServerPlayer
      turn: Turn }

type ClientPlayer =
    { selfPlayer: ClientSelfPlayer
      opponentPlayer: ClientOpponentPlayer }
