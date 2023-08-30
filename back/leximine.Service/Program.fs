open System
open System.IO
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Microsoft.Extensions.DependencyInjection
open Giraffe

printfn "Leximine Service"

open leximine.Service

let webApp =
    choose [
        route "/ping"   >=> text "pong"
        GET >=> route "/langs"    >=> DatabaseHandler.dataListHandler
        GET >=> routef "/%s/dblist" (fun ln -> DatabaseHandler.getDbListHandler ln)
        // route "/req"    >=> json { Response.result = 10
        //                            Response.success = true }
        // GET >=> route "/jopa/%i" >=> warbler (fun _ -> text "asd")
        // PUT >=> route "/data"    >=> MyHandler.myHandler

        route "/"       >=> text "Leximine Service"
    ]

let configureApp (app : IApplicationBuilder) =
    app.UseCors("AllowAll") |> ignore
    app.UseGiraffe webApp

let configureServices (services : IServiceCollection) =
    // Add Giraffe dependencies
    services.AddGiraffe().AddCors(
        fun corsPolicyBuilder ->
            corsPolicyBuilder.AddPolicy("AllowAll", fun policyBuilder ->
                policyBuilder.AllowAnyOrigin()
                             .AllowAnyMethod()
                             .AllowAnyHeader() |> ignore))
    |> ignore

[<EntryPoint>]
let main _ =
    Host.CreateDefaultBuilder()
        .ConfigureWebHostDefaults(
            fun webHostBuilder ->
                webHostBuilder
                    .Configure(configureApp)
                    .ConfigureServices(configureServices)
                    |> ignore)
        .Build()
        .Run()
    0
