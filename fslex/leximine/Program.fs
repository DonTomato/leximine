module leximineapp

open Iveonik.Stemmers

printfn "Leximine Parser"

let data = leximine.Fb2Parser.load @"d:\temp\book\children_of_time.fb2"

let result = leximine.Fb2Parser.parseData data

printfn $"Book of {result.Author}: {result.Title}"
printfn $"Paragraphs count: {result.Paragraphs |> List.length}"

printfn ""

let p = result.Paragraphs[10]

printfn $"Paragraph 10: {p}"

let words = p.Split(".") |> Seq.toList
            |> List.map (fun s -> s.Split(" ") |> Seq.toList)
            |> List.concat
            |> List.filter (fun w -> w <> "")
            |> List.map (fun w -> w.Trim())

// for word in words do
//    printfn $"%s{word}: %s{leximine.Snowball.snowballEn word}"

let stem = EnglishStemmer()

let enWords = [ "jump"; "jumping"; "jumps"; "jumped" ]

for word in enWords do
    printfn $"%s{word}: %s{stem.Stem(word)}"
