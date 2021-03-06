module Shared.Model.Game.Board

open Shared.Model.Game.Card
open Shared.Model.Game.Turn
open System

type ServerPlayer =
    { serverPlayerVanguard: Vanguard
      serverPlayerRearguard: Rearguard
      serverPlayerDeck: Deck
      serverPlayerGraveyard: Graveyard
      serverPlayerHand: Hand
      serverPlayerLife: int
      serverPlayerMana: int
      serverPlayerName: string
      serverPlayerSocketId: Option<Guid> }
    static member newPlayer(name) =
        let godTakeshiInDeck i: DeckCard =
            { name = "takeshi"
              id = i
              cardType =
                  VanguardType
                      { VanguardAttackPoint = 1
                        VanguardDefencePoint = 1 } }

        let rec duplicate (a: int) (card: DeckCard) =
            if (a > 0) then card :: (duplicate (a - 1) card) else []

        { serverPlayerVanguard = []
          serverPlayerRearguard = []
          serverPlayerDeck = duplicate 60 (godTakeshiInDeck 1)
          serverPlayerGraveyard = []
          serverPlayerHand = []
          serverPlayerLife = 80
          serverPlayerMana = 10
          serverPlayerName = name
          serverPlayerSocketId = None }

type ClientSelfPlayer =
    { selfVanguard: Vanguard
      selfRearguard: Rearguard
      selfDeck: int
      selfGraveyard: Graveyard
      selfHand: Hand
      selfLife: int
      selfMana: int
      selfName: string }

type ClientOpponentPlayer =
    { opponentVanguard: Vanguard
      opponentRearguard: Rearguard
      opponentDeck: int
      opponentGraveyard: Graveyard
      opponentHand: int
      opponentLife: int
      opponentMana: int
      opponentName: string }

type ServerBoard =
    { serverPlayer1: ServerPlayer
      serverPlayer2: ServerPlayer
      serverTurn: Turn
      roomNumber: Guid }
    static member newBoard(name1, name2) =
        { serverPlayer1 = ServerPlayer.newPlayer (name1)
          serverPlayer2 = ServerPlayer.newPlayer (name2)
          serverTurn = Standby
          roomNumber = new Guid() }

type ClientBoard =
    { clientSelfPlayer: ClientSelfPlayer
      clientOpponentPlayer: ClientOpponentPlayer
      clientTurn: Turn }

let chiralBoard (board: ServerBoard): ServerBoard =
    { board with
          serverPlayer1 = board.serverPlayer2
          serverPlayer2 = board.serverPlayer1 }

let chiralBoardOrNot (board: ServerBoard, name: string) =
    printfn "%s" name
    if board.serverPlayer1.serverPlayerName = name
    then board, id
    else if board.serverPlayer2.serverPlayerName = name
    then chiralBoard board, chiralBoard
    else raise (System.ArgumentException("Divisor cannot be zero!"))
