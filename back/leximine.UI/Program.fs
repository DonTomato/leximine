open System.Net.Mime
open Microsoft.Web.WebView2.WinForms

printfn "Leximine UI"

[<EntryPoint>]
let main args =
    // Application.EnableVisualStyles()
    let webView = new WebView2()
    webView.
    webView.CreationProperties = new CoreWebView2CreationProperties()
    0