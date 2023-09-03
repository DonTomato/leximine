module leximine.Logic.FileParsers.Main

open leximine.Logic.Helpers.Common

type BookData = {
    Title: string
    Author: string
    Sentences: string list
}

let private trimAndExcludeEmpty ls =
    ls |> List.map (fun s -> trim s) |> List.filter (fun s -> s.Length > 1)

let textToSentences (txt: string) =
    let sentences = txt
                    |> split "."
                    |> List.map (fun l -> l |> split "!")
                    |> List.concat
                    |> List.map (fun l -> l |> split "?")
                    |> List.concat
                    |> trimAndExcludeEmpty
    sentences
