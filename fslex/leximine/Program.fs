module leximineapp

printfn "Leximine Parser"

let data = leximine.Fb2Parser.load @"d:\temp\book\children_of_time.fb2"

let result = leximine.Fb2Parser.parseData data

printfn $"Book of {result.Author}: {result.Title}"
printfn $"Paragraphs count: {result.Paragraphs |> List.length}"

printfn ""

printfn $"Paragraph 10: {result.Paragraphs[10]}"