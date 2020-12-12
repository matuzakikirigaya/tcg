namespace Shared.Model.Login

open System

type Jwt = string

type LoginModel =
    { UserName: string
      Password: string
      PasswordId: Guid }
    member this.isValid() = true

type UserData =
    {
      userName: string
      token:Jwt
    }