module leximine.Service.DatabaseHandlers

open Microsoft.AspNetCore.Http
open Giraffe

let dataListHandler =
    fun (next: HttpFunc) (ctx: HttpContext) ->
        task {
            let languages = leximine.Logic.DbFileHelper.getLanguages ()
            return! json languages next ctx
        }

let getDbListHandler ln =
    fun (next: HttpFunc) (ctx: HttpContext) ->
        task {
            let dbs = leximine.Logic.DbFileHelper.getDbList ln
            let mainDb = dbs |> List.find (fun x -> x.FileName = "leximine.db")
            let response = {|
                MainDb = mainDb
                Backups = dbs |> List.filter (fun x -> x <> mainDb)
            |}
            return! json response next ctx
        }

let makeBackupHandler ln =
    fun (next: HttpFunc) (ctx: HttpContext) ->
        task {
            let backUpName = leximine.Logic.DbFileHelper.makeBackup ln
            return! json backUpName next ctx
        }

let restoreBackupHandler (ln, fileName) =
    fun (next: HttpFunc) (ctx: HttpContext) ->
        task {
            leximine.Logic.DbFileHelper.restoreBackup ln fileName
            return! Successful.OK "" next ctx
        }

let deleteBackupHandler (ln, fileName) =
    fun (next: HttpFunc) (ctx: HttpContext) ->
        task {
            leximine.Logic.DbFileHelper.deleteBackup ln fileName
            return! Successful.OK "" next ctx
        }