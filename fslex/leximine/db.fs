module leximine.db

open System
open System.Data
open Microsoft.Data.Sqlite

let createCn fileName =
    let cn = new SqliteConnection($"Data Source={fileName}")
    cn.Open()
    cn
    
let command sql (cn: SqliteConnection) =
    let command = cn.CreateCommand()
    command.CommandText <- sql
    command
    
let addParameter param value (command: SqliteCommand) =
    command.Parameters.AddWithValue(param, value)
    command
    
type SqliteReaderHelper = {
    int: string -> int
    long: string -> int64
    string: string -> string
}
    
let query f (command: SqliteCommand) =
    use reader = command.ExecuteReader()
    let tableSchema = reader.GetSchemaTable()
    let columns = tableSchema.Columns
                  |> Seq.cast<DataColumn>
                  |> Seq.map (fun c -> c.ColumnName)
                  |> Seq.toList
                  
    let getIndex columnName = columns |> List.findIndex (fun c -> c = columnName)
                  
    let r = {
        int = fun columnName -> reader.GetInt32(getIndex columnName)
        long = fun columnName -> reader.GetInt64(getIndex columnName)
        string = fun columnName -> reader.GetString(getIndex columnName)
    }
    
    seq {
        while reader.Read() do
            yield f r
    } |> Seq.toList
    
let queryScalar f (command: SqliteCommand) = f (command.ExecuteScalar())

let execute (command: SqliteCommand) =
    command.ExecuteNonQuery()
    
// Type Helpers
    
let toLong (obj: Object) = obj :?> int64
let toInt (obj: Object) = obj :?> int
let toString (obj: Object) = obj :?> string

// Helpers

let getLastID cn = cn |> command "SELECT last_insert_rowid();" |> queryScalar toLong

let transaction (cn: SqliteConnection) = cn.BeginTransaction()
let commit (transaction: SqliteTransaction) = transaction.Commit() 