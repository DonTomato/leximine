module leximine.Service.DatabaseHandler

open Microsoft.AspNetCore.Http
open Giraffe

let dataListHandler =
    fun (next: HttpFunc) (ctx: HttpContext) ->
        task {
            let languages = leximine.Logic.DbFileHelper.getLanguages ()
            return! json languages next ctx
        }
