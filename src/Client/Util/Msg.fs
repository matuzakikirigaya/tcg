module Client.Utils.Msg

open Elmish

type IMsg<'Model> =
    interface
        abstract Update: 'Model -> 'Model * Cmd<IMsg<'Model>>
    end
