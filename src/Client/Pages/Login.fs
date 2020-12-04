module Pages.Login

open Shared.Model.Login
open Thoth.Json.Net
open Fable.Core.JsInterop
open Fetch.Types

type LoginPageModel =
    { Login: LoginModel
      Running: bool
      ErrorMsg: string }

type Msg =
    | LoginSuccess of UserData
    | LoginClicked

let authUser (login: LoginModel) =
    promise {
        let body = Encode.Auto.toString (0, login)

        let props =
            [ Method HttpMethod.POST
              Fetch.requestHeaders [ ContentType "application/json" ]
              Body !^body ]

        try
            let! res = Fetch.fetch "/api/users/login/" props
            let! txt = res.text ()
            return Decode.Auto.unsafeFromString<UserData> txt
        with _ -> return! failwithf "Could not authenticate user."
    }
