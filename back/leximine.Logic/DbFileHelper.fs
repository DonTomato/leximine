module leximine.Logic.DbFileHelper

open System
open System.IO
open leximine.SqliteDb

let private root = @"c:\data\leximine\"
let private dbFileName = "leximine.db"

type DbInfo = {
    Language: string
    FileName: string
    Size: int64
}

let getLanguages = fun () ->
    Directory.GetDirectories(root)
    |> Array.map (fun x -> Path.GetFileName(x))
    |> Array.toList

let getCurrentDbFileName ln =
    Path.Combine(root, ln, dbFileName)

let initDb ln =
    let currentDbf = getCurrentDbFileName ln
    if not <| File.Exists currentDbf then
        let db = createCn currentDbf
        let currectPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)
        let sqlPath = Path.Combine(currectPath, @"..\..\..\..\..\", "sql", "initial_db.sql")
        let data = File.ReadAllText sqlPath
        db |> command data |> execute |> ignore


let getDbList ln =
    initDb ln

    Directory.GetFiles(Path.Combine(root, ln))
    |> Array.toList |> List.map (fun x ->
        let fi = new FileInfo(x)
        { DbInfo.Language = ln; FileName = fi.Name; Size = fi.Length }
    )

let makeBackup ln =
    let currentDbf = getCurrentDbFileName ln
    let newFileName = sprintf "leximine_%s.db" (DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"))
    let fullNewFileName = Path.Combine(root, ln, newFileName)
    File.Copy(currentDbf, fullNewFileName, true)
    newFileName

let restoreBackup ln fileName =
    let fullFilename = Path.Combine(root, ln, fileName)
    let currentDbf = getCurrentDbFileName ln
    File.Copy(fullFilename, currentDbf, true)
    File.Delete(fileName)

let deleteBackup ln fileName =
    let fullFilename = Path.Combine(root, ln, fileName)
    File.Delete(fullFilename)