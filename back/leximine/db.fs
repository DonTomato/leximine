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
    command.Parameters.AddWithValue(param, value) |> ignore
    command
    
type SqliteReaderHelper = {
    int: string -> int
    long: string -> int64
    string: string -> string
}
    
let query f (command: SqliteCommand) =
    use reader = command.ExecuteReader()
    let tableSchema = reader.GetSchemaTable()
                  
    let columns = tableSchema.Rows
                  |> Seq.cast<DataRow>
                  |> Seq.map (fun r -> (r.["ColumnOrdinal"] :?> int, r.["ColumnName"] :?> string))
                  |> Seq.toList
                  
    let getIndex columnName =
        let (i, _) = columns |> List.find (fun (i, c) -> c = columnName)
        i
                  
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