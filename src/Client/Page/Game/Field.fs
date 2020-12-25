module Client.Game.Field

open Fable.React
open Fable.React.Props
open Shared.Model.Game.Board
open Shared.Model.Game.Card
open Elmish
open Shared.Model.Game.ClientApi.Draw
open Shared.Model.WebSocket

type GameMsg =
    | MGetBoard
    | MGotBoard of ClientBoard
    | MDraw of DrawProps


type GameSender = GameApi -> unit

type GameModel =
    { ClientBoard: ClientBoard
      PlayerName: string
      GameSender: GameSender }
    member This.Update(msg: GameMsg): GameModel * Cmd<GameMsg> =
        match msg with
        | MGetBoard ->
            This.GameSender(GetGameBoard)
            This, Cmd.none
        | MGotBoard board -> { This with ClientBoard = board }, Cmd.none
        | MDraw props ->
            This.GameSender(Draw props)
            This, Cmd.none

    member This.View(dispatch: GameMsg -> unit) =
        let selfHandView (selfHand: list<HandCard>) =
            div [ Class "Self_hand" ] [
                div
                    [ Class "Self_hand_margin" ]
                    (List.map
                        (fun (card: HandCard) ->
                            div [ Class "Self_hand_element" ] [
                                str card.name
                            ])
                        selfHand)
            ]

        let selfVanguardView (selfVanguard: list<VanguardCard>) =
            div [ Class "Self_vanGuard" ] [
                div
                    [ Class "Self_rearGuard_margin" ]
                    (List.map
                        (fun (card: VanguardCard) ->
                            div [ Class "Self_vanGuard_element" ] [
                                str card.name
                            ])
                        selfVanguard)
            ]

        let selfRearguardView (selfRearguard: list<RearguardCard>) =
            div [ Class "Self_rearGuard" ] [
                div
                    [ Class "Self_rearGuard_margin" ]
                    (List.map
                        (fun (card: RearguardCard) ->
                            div [ Class "Self_rearGuard_element" ] [
                                str card.name
                            ])
                        selfRearguard)
            ]

        let selfGraveyardView (selfGraveyardCard: list<GraveyardCard>) =
            div [ Class "Self_graveyard" ] [
                str
                <| "墓地:"
                   + (string <| List.length selfGraveyardCard)
                   + "枚"
            ]

        let selfDeckView (selfDeck: int, playerName: string) =
            div [ Class "Self_deck"
                  OnClick(fun a -> dispatch (MDraw { playerName = playerName })) ] [
                str
                <| "デッキ:" + (string selfDeck) + "枚" + playerName
            ]

        let selfLifeManaView (selfLife, selfMana) =
            div [ Class "Self_mana" ] [
                div [] [
                    div [] [
                        str <| "ライフ" + (string selfLife)
                    ]
                    div [] [
                        str <| "マナ" + (string selfMana)
                    ]
                ]
            ]
        let opponentVanguardView (selfVanguard: list<VanguardCard>) =
            div [ Class "Self_vanGuard" ] [
                div
                    [ Class "Self_rearGuard_margin" ]
                    (List.map
                        (fun (card: VanguardCard) ->
                            div [ Class "Self_vanGuard_element" ] [
                                str <| card.name + "相手"
                            ])
                        selfVanguard)
            ]

        let opponentRearguardView (selfRearguard: list<RearguardCard>) =
            div [ Class "Self_rearGuard" ] [
                div
                    [ Class "Self_rearGuard_margin" ]
                    (List.map
                        (fun (card: RearguardCard) ->
                            div [ Class "Self_rearGuard_element" ] [
                                str <| card.name + "相手"
                            ])
                        selfRearguard)
            ]

        let opponentGraveyardView (selfGraveyardCard: list<GraveyardCard>) =
            div [ Class "Self_graveyard" ] [
                str
                <| "墓地:"
                   + (string <| List.length selfGraveyardCard)
                   + "枚"
            ]


        let opponentHandView (opponentHand: int) =
            let cards =
                let rec intToName x =
                    if x > 0 then (string x) :: (intToName (x - 1)) else [] in intToName opponentHand

            div [ Class "Self_hand" ] [
                div
                    [ Class "Self_hand_margin" ]
                    (List.map
                        (fun name ->
                            div [ Class "Self_hand_element" ] [
                                str name
                            ])
                        cards)
            ]

        let opponentDeckView (opponentDeck: int, playerName: string) =
            div [ Class "Self_deck" ] [
                str
                <| "デッキ:" + (string opponentDeck) + "枚" + playerName
            ]

        let opponentLifeManaView (opponentLife, opponentMana) =
            div [ Class "Opponent_mana" ] [
                div [] [
                    div [] [
                        str <| "ライフ" + (string opponentLife)
                    ]
                    div [] [
                        str <| "マナ" + (string opponentMana)
                    ]
                ]
            ]

        let player = This.ClientBoard.clientSelfPlayer
        let opponent = This.ClientBoard.clientOpponentPlayer

        div [] [
            div [] [
                button [ OnClick(fun ev -> dispatch MGetBoard) ] [
                    str "getBoard"
                ]
                div [ Class "Self_board" ] [
                    div [ Class "Self_field" ] [
                        opponentHandView opponent.opponentHand
                        opponentRearguardView opponent.opponentRearguard
                        opponentVanguardView opponent.opponentVanguard
                    ]
                    div [ Class "Self_zone" ] [
                        opponentGraveyardView opponent.opponentGraveyard
                        opponentDeckView (opponent.opponentDeck, This.PlayerName)
                        opponentLifeManaView (opponent.opponentLife, opponent.opponentMana)
                    ]
                ]
                div [ Class "Self_board" ] [
                    div [ Class "Self_field" ] [
                        selfVanguardView player.selfVanguard
                        selfRearguardView player.selfRearguard
                        selfHandView player.selfHand
                    ]
                    div [ Class "Self_zone" ] [
                        selfGraveyardView player.selfGraveyard
                        selfDeckView (player.selfDeck, This.PlayerName)
                        selfLifeManaView (player.selfLife, player.selfMana)
                    ]
                ]
            ]
        ]
// member this.Update msg = match msg with
// |MGetBoard ->

let gameModelInit: GameModel =
    { ClientBoard =
          { clientSelfPlayer =
                { selfDeck = 0
                  selfMana = 0
                  selfHand = []
                  selfLife = 0
                  selfGraveyard = []
                  selfRearguard = []
                  selfVanguard = [] }
            clientOpponentPlayer =
                { opponentDeck = 0
                  opponentMana = 0
                  opponentHand = 0
                  opponentLife = 0
                  opponentGraveyard = []
                  opponentRearguard = []
                  opponentVanguard = [] }
            clientTurn = Shared.Model.Game.Turn.End }
      PlayerName = ""
      GameSender = fun a -> () }
