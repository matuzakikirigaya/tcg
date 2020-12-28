module Server.Game.GameCore.Room

open Server.Game.Program
open Shared.Model.Game.Board
open Shared.Model.Game.Dummy
open System

type RoomState =
    | Matching of GameProgram
    | FindOne of string

type Rooms() =
    let mutable rooms: RoomState array = [||]

    member this.findRoom(name: string): Option<RoomState> =
        let find room =
            match room with
            | Matching program ->
                let board = program.getModel.board

                board.serverPlayer1.serverPlayerName = name
                || board.serverPlayer2.serverPlayerName = name
            | FindOne name1 -> name = name1

        Array.tryFind find rooms

    member this.matchWithNewPlayer(name: string): RoomState =
        match this.findRoom (name) with
        | Some (Matching program) -> Matching program
        | Some (FindOne name1) -> FindOne name1
        | None ->
            match Array.tryLast rooms with
            | Some (Matching _) ->
                rooms <- Array.append rooms [| FindOne name |]
                FindOne name
            | None ->
                rooms <- [| FindOne name |]
                FindOne name
            | Some (FindOne name2) ->
                let newBoard =
                    Matching(GameProgram({ board = ServerBoard.newBoard (name, name2) }, update))

                rooms.[rooms.Length - 1] <- newBoard
                newBoard

let gameRooms = Rooms()
