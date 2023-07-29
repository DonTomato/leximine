module leximine.SentenceParser

open Iveonik.Stemmers

let private split (s: string) (str: string) =
    str.Split(s) |> Seq.toList
    
let private trim (s: string) =
    s.Trim()
    
let private trimAndExcludeEmpty ls =
    ls |> List.map (fun s -> trim s) |> List.filter (fun s -> s <> "")
    
type SentenceData = {
    Sentence: string
    WordCount: int
    Words: string list
    Hash: int
}

type WordData = {
    StemmedWord: string
    Words: string list
    Count: int
}

let private enStem = EnglishStemmer()

let stem word =
    enStem.Stem(word)

let private getSentenceStatistic sentence =
    let words = sentence |> split " " |> trimAndExcludeEmpty
    {
        Sentence = sentence;
        Hash = System.HashCode.Combine(sentence);
        WordCount = List.length words
        Words = words
    }
    
let parseParagraph (p: string) =
    let sentences = p
                    |> split "."
                    |> List.map (fun l -> l |> split "!")
                    |> List.concat
                    |> List.map (fun l -> l |> split "?")
                    |> List.concat
                    |> trimAndExcludeEmpty
    sentences
    
let parseSentence (sentence: string) =
    let words = sentence |> split " " |> List.map (fun w -> w.ToLower()) |> trimAndExcludeEmpty
    words
    
let parseBook (paragraphs: string list) =
    let words = paragraphs
                |> List.map parseParagraph |> List.concat
                |> List.map parseSentence |> List.concat
                |> List.map (fun w -> (w, stem w))
                |> List.groupBy (fun (_, ws) -> ws)
                |> List.map (fun (key, values) -> (key, values |> List.length, values |> List.map (fun (w, _) -> w)))
                |> List.sortBy (fun (_, count, _) -> -count)
    words