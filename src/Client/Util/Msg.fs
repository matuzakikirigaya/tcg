module Client.Utils.Msg

open Elmish

type IMsg<'Model> =
    interface
        abstract Update: 'Model -> 'Model * Cmd<IMsg<'Model>>
    end

//*自己に影響を与えない時(viewで使わない時)に用いる
type Never<'T> = Option<'T>
