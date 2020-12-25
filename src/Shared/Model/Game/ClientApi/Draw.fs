module Shared.Model.Game.ClientApi.Draw

type DrawProps = { playerName: string }
type Msg = DrawMsg of DrawProps | DevInit