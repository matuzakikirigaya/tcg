module Client.Utils.ElmishView

open Fable.React

let inline elmishView name render = FunctionComponent.Of(render, name, equalsButFunctions)
