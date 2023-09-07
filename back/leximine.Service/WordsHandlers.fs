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

let setWordKnownHandler ln known =
    fun (next: HttpFunc) (ctx: HttpContext) ->
        task {
            let! words = ctx.BindJsonAsync<string array>()
            let wa = words |> Array.toList
            setWordKnown ln wa known
            |> ignore
            return! Successful.OK "" next ctx
        }

let setWordKnownAllBeforeHandler ln word =
    fun (next: HttpFunc) (ctx: HttpContext) ->
        task {
            setWordKnownAllBefore ln word
            |> ignore
            return! Successful.OK "" next ctx
        }