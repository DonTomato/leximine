module leximine.SentenceParser

open Iveonik.Stemmers
open leximine.Helpers
open leximine.Book

let private split (s: string) (str: string) =
    str.Split(s) |> Seq.toList
    
let private trim (s: string) =
    s.Trim()
    
    
let private excludeSymboldsFromWord = [ ","; "."; ";"; "-"; "`"; "\""; "'"; "–"; "’"; "‘"; ":"; "“"; "…"; "—" ]

let private excludeWords = [ "the"; "an"; "this"; "it";
                             "her"; "him"; "his"; "he"; "she"; "they"; "you"; "them"; "we"; "me"; "those"; "us"
                             "your"; "my";
                             
                             "is"; "was"; "were"; "have"; "has"; "had"; "be"; "are"; "been"; "will"; "would";
                             "can"; "like"; "could"; "get"; "got"; "see"; "must";
                             
                             "of"; "to"; "and"; "that"; "in"; "for"; "with"; "but"; "as"; "not"; "on"; "their";
                             "at"; "from"; "there"; "all"; "out"; "by"; "or"; "up"; "so"; "no"; "into"; "only"
                             "here"; "down"; "then"; "than"; "even"; "about"; "over"; "only"; "through"; "befor";
                             "after"; "off"; "which"; "most"; "becaus"; "already";
                             
                             "now"; "one"; "some"; "any"; "just"; "if"; "more"; "own"; "someth";
                             "time"; "other"; "go"; "back"; "come"; "though"; "do"; "any";
                             
                             "it'";
                             
                             "what"; "when"; "who"; "how"; "whi" ]

let private excludeNames = [ "holsten"; "lain"; "portia"; "guyen"; "bianca"; "karst" ]
    
let clearWord (w: string) =
    let removeFromEnd = excludeSymboldsFromWord |> List.exists (fun s -> w.EndsWith(s))
    let mutable word = w
    if removeFromEnd then
        word <- word.Substring(0, word.Length - 1)
    let removeFromStart = excludeSymboldsFromWord |> List.exists (fun s -> word.StartsWith(s))
    if removeFromStart then
        word <- word.Substring(1, word.Length - 1)
    word |> trim
    
let private trimAndExcludeEmpty ls =
    ls |> List.map (fun s -> trim s) |> List.filter (fun s -> s.Length > 1)
    
type SentenceData = {
    Sentence: string
    WordCount: int
    WordForms: string list
    Hash: string
}

type WordData = {
    StemmedWord: string
    Words: string list
    Count: int
}

type BookWordsResult = {
    WordID: string
    WordCount: int
    Words: string list
    SentenceHashes: string list
}

type BookParseResult = {
    Words: BookWordsResult list
    TotalWordsCount: int
    SentenceData: (string * SentenceData) list
}

let private enStem = EnglishStemmer()

let stemEn word =
    enStem.Stem(word)
    
let parseSentence (sentence: string) =
    let words = sentence
                |> split " "
                |> List.map (fun w -> w.ToLower())
                |> List.map (fun w -> w |> clearWord |> clearWord) 
                |> trimAndExcludeEmpty
    words

let private getSentenceStatistic sentence =
    let words = sentence |> parseSentence
    {
        Sentence = sentence;
        Hash = jenkinsHash sentence;
        WordCount = List.length words
        WordForms = words
    }
    
let parseParagraph (p: string) =
    let sentences = p
                    |> split "."
                    |> List.map (fun l -> l |> split "!")
                    |> List.concat
                    |> List.map (fun l -> l |> split "?")
                    |> List.concat
                    |> trimAndExcludeEmpty
    sentences |> List.map getSentenceStatistic
    
// Obsolete
let parseBook (paragraphs: string list) =
    let sentences = paragraphs
                    |> List.map parseParagraph |> List.concat
                    
    let sentencesData = sentences
                        |> List.groupBy (fun ss -> ss.Hash)
                        |> List.map (fun (key, values) -> (key, values[0]))
                    
    let words = sentences
                |> List.map (fun ss -> ss.WordForms |> List.map (fun w -> (w, ss.Hash))) |> List.concat
                
    let data = words
               |> List.map (fun (w, ss) -> (w, stemEn w, ss))
               |> List.filter (fun (_, ws, _) -> not (excludeWords |> List.contains ws))
               |> List.filter (fun (_, ws, _) -> not (excludeNames |> List.contains ws))
               |> List.groupBy (fun (_, ws, _) -> ws)
               |> List.map (fun (key, values) -> {
                   BookWordsResult.WordID = key
                   WordCount = values |> List.length
                   Words = values |> List.map (fun (w, _, _) -> w) |> List.distinct
                   SentenceHashes = values |> List.map (fun (_, _, ss) -> ss)
               })
               |> List.sortBy (fun x -> -x.WordCount)
               
    {
        BookParseResult.Words = data
        TotalWordsCount = words |> Seq.length
        SentenceData = sentencesData 
    }
    
let parseBookToSentenceStatisic (paragraphs: string list) =
    let result = paragraphs
                 |> List.map parseParagraph |> List.concat
    
    result
    
let getNewWords existingWords (stem: StemFn) nSentences =
    let result = nSentences
                 |> List.map (fun s -> s.WordForms)
                 |> List.concat
                 |> List.map (fun s -> stem s)
                 |> List.distinct
                 |> List.filter (fun w -> not (existingWords |> List.contains w))
                 // |> List.groupBy (fun s -> s)
                 // |> List.map (fun (key, values) -> (key, values |> List.length))
                 
    result
    
let getNewWordForms existingWordForms (stem: StemFn) nSentences =
    nSentences
    |> List.map (fun s -> s.WordForms)
    |> List.concat
    |> List.distinct
    |> List.filter (fun w -> not (existingWordForms |> List.contains w))
    |> List.map (fun w -> (w, stem w))
    
let getNewSentences existingHashes nSentences =
    let result = nSentences
                 |> List.filter (fun s -> not (existingHashes |> List.contains s.Hash))
                 |> List.groupBy (fun s -> s.Hash)
                 |> List.map (fun (key, values) -> {
                     Hash = key
                     Sentence = values[0].Sentence
                     WordCount = values[0].WordCount
                     WordForms = values[0].WordForms
                 })
                 
    result