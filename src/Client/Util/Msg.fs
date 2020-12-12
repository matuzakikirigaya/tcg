module Client.Utils.Msg

open Elmish

type IMsg<'T> =
    interface
        abstract Update: 'T -> 'T * Cmd<IMsg<'T>>
    end
