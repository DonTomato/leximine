module leximine.Helpers

open System.Data.HashFunction.Jenkins
open System.Text

let private instance = JenkinsOneAtATimeFactory.Instance.Create()

let jenkinsHash (s: string) =
    let encoding = Encoding.UTF8
    let bytes = encoding.GetBytes(s)
    instance.ComputeHash(bytes).AsHexString()
    
let log label value =
    printfn "%-50s %7i" (sprintf "%s:" label) value 