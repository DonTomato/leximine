module leximine.Service.DatabaseHandlers

open Microsoft.AspNetCore.Http
open Giraffe
open leximine.Logic.Services.DbFileServices

let dataListHandler =
    fun (next: HttpFunc) (ctx: HttpContext) ->
        task {
            let languages = getLanguages ()
            return! json languages next ctx
        }

let getDbListHandler ln =
    fun (next: HttpFunc) (ctx: HttpContext) ->
        task {
            let dbs = getDbList ln
            let mainDb = dbs |> List.find (fun x -> x.FileName = "leximine.db")
            let response = {|
                MainDb = mainDb
                Backups = dbs |> List.filter (fun x -> x <> mainDb) |> List.sortByDescending (fun x -> x.FileName)
            |}
            return! json response next ctx
        }

let makeBackupHandler ln =
    fun (next: HttpFunc) (ctx: HttpContext) ->
        task {
            let backUpName = makeBackup ln
            return! json backUpName next ctx
        }

let restoreBackupHandler (ln, fileName) =
    fun (next: HttpFunc) (ctx: HttpContext) ->
        task {
            restoreBackup ln fileName
            return! Successful.OK "" next ctx
        }

let deleteBackupHandler (ln, fileName) =
    fun (next: HttpFunc) (ctx: HttpContext) ->
        task {
            deleteBackup ln fileName
            return! Successful.OK "" next ctx
        }