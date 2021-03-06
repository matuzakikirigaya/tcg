module Shared.Model.Game.GameElmish

open Shared.Model.Game.Board
open System

type sGetGameBoardApi = { playerName: string; socketId: Guid }

type GameMsg =
    | Draw of Shared.Model.Game.ClientApi.SimplyName.SimplyName
    | DevInit
    | SGetGameBoard of sGetGameBoardApi

type GameDispatch<'Msg> = 'Msg -> unit
type GameSub<'Msg> = GameDispatch<'Msg> -> unit
type GameCmd<'Msg> = list<GameSub<'Msg>>

type GameModel = { board: ServerBoard }
type GameUpdate = GameModel * GameMsg -> GameModel * GameCmd<GameMsg>

module GameCmd =
    let map (f: 'a -> 'msg) (cmd: GameCmd<'a>): GameCmd<'msg> =
        cmd
        |> List.map (fun g -> (fun dispatch -> f >> dispatch) >> g)
