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
type HandCard =
    { id: int
      name: string
      cardType: CardType }
type GraveyardCard =
    { id: int
      name: string
      cardType: CardType }
type VanguardCard = { id: int; name: string }
type RearguardCard = { id: int; name: string }
type Rearguard = List<RearguardCard>
type Vanguard = List<VanguardCard>
type Graveyard = List<GraveyardCard>
type Deck = List<DeckCard>
type Hand = List<HandCard>

module CardModule =
    let DCtoHC (deck:DeckCard):HandCard = {
        id = deck.id
        name = deck.name
        cardType = deck.cardType
      }