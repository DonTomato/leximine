module leximineapp

open Iveonik.Stemmers

printfn "Leximine Parser"

let book = @"d:\temp\book\children_of_time.fb2"
let dbPath = @"C:\data\leximine_dev.db"
let enStem = EnglishStemmer()
let stem s = enStem.Stem(s)

leximine.BookStatisticSaver.processBook book dbPath stem
