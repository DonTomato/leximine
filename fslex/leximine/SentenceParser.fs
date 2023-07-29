module leximine.SentenceParser

let split (s: string) (str: string) =
    str.Split(s) |> Seq.toList
    
let trim (s: string) =
    s.Trim()
    
let trimAndExcludeEmpty ls =
    ls |> List.map (fun s -> trim s) |> List.filter (fun s -> s <> "")
    
let parseParagraph (p: string) =
    let sentences = p
                    |> split "."
                    |> List.map (fun l -> l |> split "!")
                    |> List.concat
                    |> List.map (fun l -> l |> split "?")
                    |> List.concat
                    |> trimAndExcludeEmpty
    sentences
    