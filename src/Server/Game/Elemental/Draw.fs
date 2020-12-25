module Server.Game.Elemental.DrawDH

open Shared.Model.Game.Card

let Draw (hoge: Deck): Result<HandCard * Deck, string> =
    match hoge with
    | x :: xs -> Ok(CardModule.DCtoHC x, xs)
    | _ -> Error "アウト"
