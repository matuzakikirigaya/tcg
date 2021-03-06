module Client.Game.Field

open Fable.React
open Fable.React.Props
open Shared.Model.Game.Board
open Shared.Model.Game.Card
open Elmish
open Shared.Model.WebSocket
open Shared.Model.Game.ClientApi.SimplyName

type GameMsg =
    | MGetBoard
    | MGotBoard of ClientBoard
    | MDraw
    | MDevInit

type GameSender = GameApi -> unit

type GameModel =
    { ClientBoard: ClientBoard
      PlayerName: string
      GameSender: GameSender }
    member This.Update(msg: GameMsg): GameModel * Cmd<GameMsg> =
        match msg with
        | MGetBoard ->
            This.GameSender(GetGameBoard { playerName = This.PlayerName })
            This, Cmd.none
        | MGotBoard board -> { This with ClientBoard = board }, Cmd.none
        | MDraw ->
            This.GameSender(Draw { playerName = This.PlayerName })
            This, Cmd.none
        | MDevInit ->
            This.GameSender(DevInit { playerName = This.PlayerName })
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

        let selfDeckView (selfDeck: int) =
            div [ Class "Self_deck"
                  OnClick(fun a -> dispatch (MDraw )) ] [
                str <| "デッキ:" + (string selfDeck) + "枚"
            ]

        let selfLifeManaView (selfLife, selfMana, playerName) =
            div [ Class "Self_mana" ] [
                div [] [
                    div [] [
                        str <| "ライフ:" + (string selfLife)
                    ]
                    div [] [
                        str <| "マナ:" + (string selfMana)
                    ]
                    div [] [ str <| "プレイヤー:" + playerName ]
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
                    if x > 0 then (string x) :: (intToName (x - 1)) else [] in

                intToName opponentHand

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

        let opponentDeckView (opponentDeck: int) =
            div [ Class "Self_deck" ] [
                str <| "デッキ:" + (string opponentDeck) + "枚"
            ]

        let opponentLifeManaView (opponentLife, opponentMana, playerName) =
            div [ Class "Opponent_mana" ] [
                div [] [
                    div [] [
                        str <| "ライフ:" + (string opponentLife)
                    ]
                    div [] [
                        str <| "マナ:" + (string opponentMana)
                    ]
                    div [] [ str <| "プレイヤー:" + playerName ]
                ]
            ]

        let player = This.ClientBoard.clientSelfPlayer
        let opponent = This.ClientBoard.clientOpponentPlayer

        div [] [
            div [] [
                button [ OnClick(fun ev -> dispatch MGetBoard) ] [
                    str "getBoard"
                ]
                button [ OnClick(fun ev -> dispatch MDevInit) ] [
                    str "devInit"
                ]
                div [ Class "Self_board" ] [
                    div [ Class "Self_field" ] [
                        opponentHandView opponent.opponentHand
                        opponentRearguardView opponent.opponentRearguard
                        opponentVanguardView opponent.opponentVanguard
                    ]
                    div [ Class "Self_zone" ] [
                        opponentGraveyardView opponent.opponentGraveyard
                        opponentDeckView (opponent.opponentDeck)
                        opponentLifeManaView (opponent.opponentLife, opponent.opponentMana, opponent.opponentName)
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
                        selfDeckView (player.selfDeck)
                        selfLifeManaView (player.selfLife, player.selfMana, player.selfName)
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
                  selfName = ""
                  selfVanguard = [] }
            clientOpponentPlayer =
                { opponentDeck = 0
                  opponentMana = 0
                  opponentHand = 0
                  opponentLife = 0
                  opponentGraveyard = []
                  opponentRearguard = []
                  opponentName = ""
                  opponentVanguard = [] }
            clientTurn = Shared.Model.Game.Turn.End }
      PlayerName = ""
      GameSender = fun a -> () }
