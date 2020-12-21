module Client.Game.Field

open Fable.React
open Fable.React.Props

let view =
    div [] [

        div [] [
            div [ Class "Self_board" ] [
                div [ Class "Self_field" ] [
                    div [ Class "Self_hand" ] [
                        div [ Class "Self_hand_margin" ] [
                            div [ Class "Self_hand_element" ] [
                                str "手札神"
                            ]
                            div [ Class "Self_hand_element" ] [
                                str "手札神2"
                            ]
                            div [ Class "Self_hand_element" ] [
                                str "手札神3"
                            ]
                            div [ Class "Self_hand_element" ] [
                                str "手札神4"
                            ]
                            div [ Class "Self_hand_element" ] [
                                str "手札神5"
                            ]
                        ]
                    ]
                    div [ Class "Self_rearGuard" ] [
                        div [ Class "Self_rearGuard_margin" ] [
                            div [ Class "Self_rearGuard_element" ] [
                                str "後衛ミカエル"
                            ]
                            div [ Class "Self_rearGuard_element" ] [
                                str "後衛ダビデ"
                            ]
                            div [ Class "Self_rearGuard_element" ] [
                                str "後衛ダビデ"
                            ]
                            div [ Class "Self_rearGuard_element" ] [
                                str "後衛ダビデ"
                            ]
                        ]
                    ]
                    div [ Class "Self_vanGuard_margin" ] [
                        div [ Class "Self_vanGuard" ] [
                            div [ Class "Self_vanGuard_element" ] [
                                str "前衛堕天使"
                            ]
                            div [ Class "Self_vanGuard_element" ] [
                                str "前衛堕天使2"
                            ]
                            div [ Class "Self_vanGuard_element" ] [
                                str "前衛堕天使"
                            ]
                            div [ Class "Self_vanGuard_element" ] [
                                str "前衛堕天使"
                            ]
                            div [ Class "Self_vanGuard_element" ] [
                                str "前衛堕天使"
                            ]
                        ]
                    ]
                ]
                div [ Class "Self_zone" ] [
                    div [ Class "Self_graveyard" ] [
                        str "自分の墓地"
                    ]
                    div [ Class "Self_deck" ] [
                        str "デッキ40枚"
                    ]
                    div [ Class "Self_mana" ] [
                        div [] [
                            div [] [ str "ライフ30" ]
                            div [] [ str "マナ赤1枚" ]
                        ]
                    ]
                ]
            ]
            div [ Class "Self_board" ] [
                div [ Class "Self_field" ] [
                    div [ Class "Self_vanGuard_margin" ] [
                        div [ Class "Self_vanGuard" ] [
                            div [ Class "Self_vanGuard_element" ] [
                                str "前衛堕天使"
                            ]
                            div [ Class "Self_vanGuard_element" ] [
                                str "前衛堕天使2"
                            ]
                            div [ Class "Self_vanGuard_element" ] [
                                str "前衛堕天使"
                            ]
                            div [ Class "Self_vanGuard_element" ] [
                                str "前衛堕天使"
                            ]
                            div [ Class "Self_vanGuard_element" ] [
                                str "前衛堕天使"
                            ]
                        ]
                    ]
                    div [ Class "Self_rearGuard" ] [
                        div [ Class "Self_rearGuard_margin" ] [
                            div [ Class "Self_rearGuard_element" ] [
                                str "後衛ミカエル"
                            ]
                            div [ Class "Self_rearGuard_element" ] [
                                str "後衛ダビデ"
                            ]
                            div [ Class "Self_rearGuard_element" ] [
                                str "後衛ダビデ"
                            ]
                            div [ Class "Self_rearGuard_element" ] [
                                str "後衛ダビデ"
                            ]
                        ]
                    ]
                    div [ Class "Self_hand" ] [
                        div [ Class "Self_hand_margin" ] [
                            div [ Class "Self_hand_element" ] [
                                str "手札神"
                            ]
                            div [ Class "Self_hand_element" ] [
                                str "手札神2"
                            ]
                            div [ Class "Self_hand_element" ] [
                                str "手札神3"
                            ]
                            div [ Class "Self_hand_element" ] [
                                str "手札神4"
                            ]
                            div [ Class "Self_hand_element" ] [
                                str "手札神5"
                            ]
                        ]
                    ]
                ]
                div [ Class "Self_zone" ] [
                    div [ Class "Self_mana" ] [
                        div [] [
                            div [] [ str "マナ赤1枚" ]
                            div [] [ str "ライフ30" ]
                        ]
                    ]
                    div [ Class "Self_graveyard" ] [
                        str "自分の墓地"
                    ]
                    div [ Class "Self_deck" ] [
                        str "デッキ40枚"
                    ]
                ]
            ]
        ]
    ]
