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
