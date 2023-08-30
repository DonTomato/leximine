module leximine.Logic.DbFileHelper

open System
open System.IO
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
        let originalDbf = Path.Combine(root, dbFileName)
        File.Copy(originalDbf, currentDbf)

let getDbList ln =
    initDb ln

    Directory.GetFiles(Path.Combine(root, ln))
    |> Array.map (fun x ->
        let fi = new FileInfo(x)
        { Language = ln; FileName = x; Size = fi.Length }
    )

let makeBackup ln =
    let currentDbf = getCurrentDbFileName ln
    let newFileName = sprintf "leximine_%s.db" (DateTime.Now.ToString("yyyyMMddHHmmss"))
    File.Copy(currentDbf, newFileName, true)
    newFileName

let restoreBackup ln fileName =
    let fullFilename = Path.Combine(root, ln, fileName)
    let currentDbf = getCurrentDbFileName ln
    File.Copy(fullFilename, currentDbf, true)
    File.Delete(fileName)