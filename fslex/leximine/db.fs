module leximine.db

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

let execute (command: SqliteCommand) =
    command.ExecuteNonQuery()
