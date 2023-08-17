module leximine.Service.MyHandler

open Microsoft.AspNetCore.Http
open Giraffe

type MyRequest = {
    UserId: string;
    A: int;
    B: int;
}

type MyResponse = {
    Result: int
    Success: bool
}

let myHandler =
    fun (next: HttpFunc) (ctx: HttpContext) ->
        task {
            let! person = ctx.BindFormAsync<MyRequest>()
            let result = {
                Result = person.A + person.B
                Success = true 
            }
            return! json result next ctx
        }