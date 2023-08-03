module leximine.db

open System
open Microsoft.Data.Sqlite

let createCn fileName =
    let cn = new SqliteConnection($"Data Source={fileName}")
    cn.Open()
    cn
    
let commnd sql (cn: SqliteConnection) =
    let command = cn.CreateCommand()
    command.CommandText <- sql
    command
    
let addParameter param value (command: SqliteCommand) =
    command.Parameters.AddWithValue(param, value)
    command
    
let query f (command: SqliteCommand) =
    use reader = command.ExecuteReader()
    seq {
        while reader.Read() do
            yield f reader
    } |> Seq.toList
    
let queryScalar f (command: SqliteCommand) = f (command.ExecuteScalar())

let execute (command: SqliteCommand) =
    command.ExecuteNonQuery()
    
// Type Helpers
    
let toLong (obj: Object) = obj :?> int64
let toInt (obj: Object) = obj :?> int
let toString (obj: Object) = obj :?> string

// Helpers

let getLastID cn = cn |> commnd "SELECT last_insert_rowid();" |> queryScalar toLong

let transaction (cn: SqliteConnection) = cn.BeginTransaction()
let commit (transaction: SqliteTransaction) = transaction.Commit() 