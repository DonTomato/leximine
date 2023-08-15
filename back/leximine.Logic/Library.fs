module leximine.Helpers

open System.Data.HashFunction.Jenkins
open System.Text
open System.Text.RegularExpressions

let private instance = JenkinsOneAtATimeFactory.Instance.Create()

let jenkinsHash (s: string) =
    let encoding = Encoding.UTF8
    let bytes = encoding.GetBytes(s)
    instance.ComputeHash(bytes).AsHexString()
    
let log label value =
    printfn "%-50s %7i" (sprintf "%s:" label) value


    
// String helpers

let internal split (s: string) (str: string) =
    str.Split(s) |> Seq.toList
    
let internal splitToWords (s: string) =
    let regex = new Regex(@"\w+")
    regex.Matches(s)
    |> Seq.cast<Match>
    |> Seq.map (fun m -> m.Value)
    |> Seq.toList
    
let internal trim (s: string) =
    s.Trim()
    
let internal toLower (s: string) =
    s.ToLowerInvariant()