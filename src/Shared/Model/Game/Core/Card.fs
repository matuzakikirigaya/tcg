module Shared.Model.Game.Card

type VanguardType =
    { VanguardAttackPoint: int
      VanguardDefencePoint: int }

type RearguardType =
    { RearguardAttackPoint: int
      RearguardDefencePoint: int }

type CardType =
    | VanguardType of VanguardType
    | RearguardType of RearguardType

type DeckCard =
    { id: int
      name: string
      cardType: CardType }

type Deck = List<DeckCard>

type HandCard =
    { id: int
      name: string
      cardType: CardType }

type Hand = List<HandCard>

type GraveyardCard =
    { id: int
      name: string
      cardType: CardType }
type Graveyard = List<GraveyardCard>

type VanguardCard = { id: int; name: string }
type Vanguard = List<VanguardCard>

type RearguardCard = { id: int; name: string }
type Rearguard = List<RearguardCard>
