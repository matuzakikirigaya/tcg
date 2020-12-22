module Shared.Model.Game.Card

type VanguardType =
    | VanguardAttackPoint of int
    | VanguardDefencePoint of int

type RearguardType =
    | RearguardAttackPoint of int
    | RearguardDefencePoint of int

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
