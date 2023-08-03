module leximineapp

open Iveonik.Stemmers
open leximine.DbLeximine
open leximine.db

printfn "Leximine Parser"

let data = leximine.Fb2Parser.load @"d:\temp\book\children_of_time.fb2"

let bookData = leximine.Fb2Parser.parseData data

printfn $"Book of {bookData.Author}: {bookData.Title}"
printfn $"Paragraphs count: {bookData.Paragraphs |> List.length}"

printfn ""

let result = leximine.SentenceParser.parseBook bookData.Paragraphs

let uniqueWordsCount = result.Words |> List.length
let words100 = result.Words |> List.skip 4000 |> List.take 100

printfn "Count of words: %i" result.TotalWordsCount
printfn "Count of unique words: %i" uniqueWordsCount
printfn ""

for wStat in words100 do
    printfn "%5i: %20s:  %A" wStat.WordCount wStat.WordID wStat.Words  
    
    
let w = result.Words[4016]
printfn ""
printfn $"Words for index 4000: stemmed: {w.WordID}"
printfn $"Words used {leximine.SentenceParser.clearWord w.WordID}"

let sentences4000 = result.SentenceData |> List.filter (fun (h, _) -> w.SentenceHashes |> List.contains h)
                                        |> List.map (fun (_, s) -> s.Sentence) 

printfn "Sentences"

for s in sentences4000 do
    printfn "  %s" s
    
// DataBase

use cn = leximine.db.createCn @"C:\data\leximine_dev.db"

let book = {
    DbBook.Title = bookData.Title
    Author = bookData.Author
    Lang = Language.English
    TotalWordsCount = result.TotalWordsCount
    TotalSentenceCount = (result.SentenceData |> List.length)
    UniqueWordsCount = uniqueWordsCount 
}

let tr = cn |> transaction

let bookId = cn |> saveBook book

let sentences = result.SentenceData
                |> List.map (fun (_, s) -> {
                    DbSentence.Sentence = s.Sentence
                    Hash = s.Hash.ToString("x")
                    BookId = bookId
                    WordsCount = s.WordCount
                    Lang = Language.English 
                })
                |> List.map (fun s ->
                    let sid = cn |> saveSentence s
                    (sid, s.Hash, s))

tr |> commit