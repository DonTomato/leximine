module leximineapp

printfn "Leximine Parser"

let data = leximine.Fb2Parser.load @"d:\temp\book\children_of_time.fb2"

let result = leximine.Fb2Parser.parseData data

printf "%O" result