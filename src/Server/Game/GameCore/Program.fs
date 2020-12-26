module Server.Game.Program

open Shared.Model.Game.GameElmish
open Shared.Model.Game.Dummy

type GameProgram(init: GameModel, update: GameUpdate) =
    let update = update
    let mutable innerModel = init
    member this.getModel = innerModel

    member this.dispatch msg =
        let (model', cmd1) = update (innerModel, msg)
        List.iter (fun call -> call this.dispatch) cmd1
        innerModel <- model'

let update: GameUpdate =
    fun (model, msg) ->
        match msg with
        | Draw draw ->
            let board, list =
                Server.Game.Usecases.Draw.drawUpdate (model.board, draw)

            { board = board }, GameCmd.map Draw list
        | DevInit ->
            let board, cmd = Usecases.DevInit.devInitUpdate
            { board = board }, cmd

let program =
    GameProgram({ board = initialServerBoard }, update)
