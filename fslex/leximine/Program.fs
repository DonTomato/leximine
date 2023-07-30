module leximineapp

open Iveonik.Stemmers

printfn "Leximine Parser"

let data = leximine.Fb2Parser.load @"d:\temp\book\children_of_time.fb2"

let result = leximine.Fb2Parser.parseData data

printfn $"Book of {result.Author}: {result.Title}"
printfn $"Paragraphs count: {result.Paragraphs |> List.length}"

printfn ""

// let p = result.Paragraphs[10]
//
// printfn $"Paragraph 10: {p}"
//
// let words = p.Split(".") |> Seq.toList
//             |> List.map (fun s -> s.Split(" ") |> Seq.toList)
//             |> List.concat
//             |> List.filter (fun w -> w <> "")
//             |> List.map (fun w -> w.Trim())
//
// // for word in words do
// //    printfn $"%s{word}: %s{leximine.Snowball.snowballEn word}"
//
// let stem = EnglishStemmer()
//
// let enWords = [ "jump"; "jumping"; "jumps"; "jumped" ]
//
// for word in enWords do
//     printfn $"%s{word}: %s{stem.Stem(word)}"

let (words, wordsCount) = leximine.SentenceParser.parseBook result.Paragraphs

let uniqueWordsCount = words |> List.length
let words100 = words |> List.skip 4000 |> List.take 100

printfn "Count of words: %i" wordsCount
printfn "Count of unique words: %i" uniqueWordsCount
printfn ""

for (w, count, wu) in words100 do
    printfn "%5i: %20s:  %A" count w wu  
    
    
let (ws, wsCount, wordsUsed) = words[4016]
printfn ""
printfn $"Words for index 4000: stemmed: {ws}"
printfn $"Words used {leximine.SentenceParser.clearWord ws}"