module leximine.Service.DatabaseHandler

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
            let languages = leximine.Logic.DbFileHelper.getDbList ln
            return! json languages next ctx
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