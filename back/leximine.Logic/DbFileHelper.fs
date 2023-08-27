module leximine.Logic.DbFileHelper

open System.IO
let private root = @"c:\data\leximine\"

let getLanguages = fun () ->
    Directory.GetDirectories(root)
    |> Array.map (fun x -> Path.GetFileName(x))
    |> Array.toList