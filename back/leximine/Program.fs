module leximineapp

open System.Text
open Iveonik.Stemmers

printfn "Leximine Parser"

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance)

let book = @"G:\My Drive\Storage\LeximineData\En\children_of_time.fb2"
// let book = @"d:\temp\book\1.fb2";
// let book = @"d:\temp\book\2.fb2";
// let book = @"d:\temp\book\3.fb2";
// let book = @"d:\temp\book\4.fb2";
let dbPath = @"C:\Data\leximine\en\leximine.db"
let enStem = EnglishStemmer()
let stem s = enStem.Stem(s)

leximine.BookStatisticSaver.processBook book dbPath stem
