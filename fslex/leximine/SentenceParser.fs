module leximine.SentenceParser

open Iveonik.Stemmers
open leximine.Helpers
open leximine.Book
    
    
let private excludeSymboldsFromWord = [ ","; "."; ";"; "-"; "`"; "\""; "'"; "–"; "’"; "‘"; ":"; "“"
                                        "…"; "—"; "("; ")"; "’"; ]

// let private excludeWords = [ "the"; "an"; "this"; "it";
//                              "her"; "him"; "his"; "he"; "she"; "they"; "you"; "them"; "we"; "me"; "those"; "us"
//                              "your"; "my";
//                              
//                              "is"; "was"; "were"; "have"; "has"; "had"; "be"; "are"; "been"; "will"; "would";
//                              "can"; "like"; "could"; "get"; "got"; "see"; "must";
//                              
//                              "of"; "to"; "and"; "that"; "in"; "for"; "with"; "but"; "as"; "not"; "on"; "their";
//                              "at"; "from"; "there"; "all"; "out"; "by"; "or"; "up"; "so"; "no"; "into"; "only"
//                              "here"; "down"; "then"; "than"; "even"; "about"; "over"; "only"; "through"; "befor";
//                              "after"; "off"; "which"; "most"; "becaus"; "already";
//                              
//                              "now"; "one"; "some"; "any"; "just"; "if"; "more"; "own"; "someth";
//                              "time"; "other"; "go"; "back"; "come"; "though"; "do"; "any";
//                              
//                              "it'";
//                              
//                              "what"; "when"; "who"; "how"; "whi" ]

let private excludeWords = [ "don" ]

let private excludeNames = [ "Holsten"; "Lain"; "Portia"; "Guyen"; "Bianca"; "Karst"; "Vitas"; "Scoles"; "Avrana"; "Viola"; "Alpash"; "Nessel" ]
    
let private trimAndExcludeEmpty ls =
    ls |> List.map (fun s -> trim s) |> List.filter (fun s -> s.Length > 1)
    
type SentenceWord = {
    WordID: string
    WordForm: string
}
    
type SentenceData = {
    Sentence: string
    WordCount: int
    WordForms: SentenceWord list
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
                |> splitToWords
                |> List.except excludeNames
                |> List.map (fun w -> toLower w)
                |> trimAndExcludeEmpty
    words

let private getSentenceStatistic (stem: StemFn) sentence =
    let words = sentence |> parseSentence
    {
        Sentence = sentence;
        Hash = jenkinsHash sentence;
        WordCount = List.length words
        WordForms = words |> List.map (fun w -> {
            WordID = stem w
            WordForm = w 
        })
    }
    
let parseParagraph (stem: StemFn) (p: string) =
    let sentences = p
                    |> split "."
                    |> List.map (fun l -> l |> split "!")
                    |> List.concat
                    |> List.map (fun l -> l |> split "?")
                    |> List.concat
                    |> trimAndExcludeEmpty
    sentences |> List.map (fun s -> getSentenceStatistic stem s)
    
let parseBookToSentenceStatistic (paragraphs: string list) (stem: StemFn) =
    let result = paragraphs
                 |> List.map (fun p -> parseParagraph stem p) |> List.concat
    
    result
    
let getNewWords existingWords nSentences =
    
    let getDefault wid words =
        if words |> List.contains wid then
            wid
        else
            words
            |> List.sortBy (fun s -> String.length(s))
            |> List.head
        
    
    let result = nSentences
                 |> List.map (fun s -> s.WordForms)
                 |> List.concat
                 |> List.groupBy (fun s -> s.WordID)
                 |> List.map (fun (w, values) -> (w, values
                                                     |> List.map (fun v -> v.WordForm)
                                                     |> List.distinct
                                                     |> getDefault w))
                 |> List.distinct
                 |> List.filter (fun (w, _) -> not (existingWords |> List.contains w))
                 
    result
    
let getNewWordForms existingWordForms nSentences =
    nSentences
    |> List.map (fun s -> s.WordForms)
    |> List.concat
    |> List.groupBy (fun w -> w.WordForm)
    |> List.map (fun (_, values) -> values[0])
    |> List.filter (fun w -> not (existingWordForms |> List.contains w.WordForm))
    
let getNewSentences existingHashes nSentences =
    nSentences
    |> List.filter (fun s -> not (existingHashes |> List.contains s.Hash))
    |> List.groupBy (fun s -> s.Hash)
    |> List.map (fun (key, values) -> {
        Hash = key
        Sentence = values[0].Sentence
        WordCount = values[0].WordCount
        WordForms = values[0].WordForms
    })
    
let getWordIDs sentenceStat =
    sentenceStat
    |> List.map (fun s -> s.WordForms)
    |> List.concat
    |> List.groupBy (fun s -> s.WordID)
    |> List.map (fun (id, values) -> (id, values |> List.length))
    
let getWordForms sentenceStat =
    sentenceStat
    |> List.map (fun s -> s.WordForms)
    |> List.concat
    |> List.groupBy (fun s -> s.WordForm)
    |> List.map (fun (id, values) -> (id, values |> List.length))
    
let getWordFormSentence existing sentenceStat =
    sentenceStat
    |> List.map (fun s -> s.WordForms
                          |> List.map (fun wf -> (wf.WordForm, s.Hash)))
    |> List.concat
    |> List.distinct
    |> List.except existing
    
let getWordSentence existing sentenceStat =
    sentenceStat
    |> List.map (fun s -> s.WordForms
                          |> List.map (fun e -> (e.WordID, s.Hash)))
    |> List.concat
    |> List.distinct
    |> List.except existing