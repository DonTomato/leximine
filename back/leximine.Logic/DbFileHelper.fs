module leximine.Logic.DbFileHelper

open System.IO
let private root = @"c:\data\leximine\"
let private dbFileName = "leximine.db"

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