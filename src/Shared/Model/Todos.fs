namespace Shared.Model.Todos

open System

type Todo = { Id: Guid; Description: string }
type TodosModel = { Todos: Todo list; Input: string }
