open System
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
        route "/req"    >=> json { Response.result = 10
                                   Response.success = true }
        GET >=> route "/jopa/%i" >=> warbler (fun _ -> text "asd")
        PUT >=> route "/data"    >=> MyHandler.myHandler
        
        route "/"       >=> htmlFile "/pages/index.html"
    ]

let configureApp (app : IApplicationBuilder) =
    // Add Giraffe to the ASP.NET Core pipeline
    app.UseGiraffe webApp

let configureServices (services : IServiceCollection) =
    // Add Giraffe dependencies
    services.AddGiraffe() |> ignore

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
