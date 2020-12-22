module Client.Game.Field

open Fable.React
open Fable.React.Props
open Shared.Model.Game.Board
open Shared.Model.Game.Card

type GameMsg = MGetBoard

open Elmish

type GameModel =
    { ClientBoard: ClientBoard }
    member This.Update(msg: GameMsg): GameModel * Cmd<GameMsg> =
        match msg with
        | MGetBoard -> This, Cmd.none

    member This.View a =
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
                str (string <| List.length selfGraveyardCard)
            ]

        let selfDeckView (selfDeck: int) =
            div [ Class "Self_deck" ] [
                str (string selfDeck)
            ]

        let selfLifeManaView (selfMana, selfLife) =
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

        let player = This.ClientBoard.clientSelfPlayer

        div [] [
            div [] [
                div [ Class "Self_board" ] [
                    div [ Class "Self_field" ] [
                        selfHandView player.selfHand
                        selfVanguardView player.selfVanguard
                        selfRearguardView player.selfRearguard
                    ]
                    div [ Class "Self_zone" ] [
                        selfGraveyardView player.selfGraveyard
                        selfDeckView player.selfDeck
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
            clientTurn = Shared.Model.Game.Turn.End } }
