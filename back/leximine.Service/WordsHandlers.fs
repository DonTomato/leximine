module leximine.Service.WordsHandlers

open Giraffe
open Microsoft.AspNetCore.Http
open leximine.Logic.Services.WordsServices

let geInitDataHandler ln =
    fun (next: HttpFunc) (ctx: HttpContext) ->
        task {
            let data = getInitData ln
            return! json data next ctx
        }

let getWordsHandler ln filter =
    fun (next: HttpFunc) (ctx: HttpContext) ->
        task {
            let data = getWordsPage ln filter
            return! json data next ctx
        }