module leximine.DbLeximine

open leximine.db

type Language =
    | English
    | Norwegian

type DbBook = {
    Title: string
    Author: string
    Lang: Language
    TotalWordsCount: int
    TotalSentenceCount: int
    UniqueWordsCount: int
}

type DbSentence = {
    Sentence: string
    Hash: string
    WordsCount: int
}

let private getStrLang lang =
    match lang with
    | English -> "en"
    | Norwegian -> "no"

let saveBook book cn =
        let code = cn
                   |> commnd @"
                        INSERT INTO book (lang, title, author, words_count, sentence_count, unique_words_count)
                        VALUES ($lang, $title, $author, $wc, $sc, $uwc)"
                   |> addParameter "$lang" (getStrLang book.Lang)
                   |> addParameter "$title" book.Title
                   |> addParameter "$author" book.Author
                   |> addParameter "$wc" book.TotalWordsCount
                   |> addParameter "$sc" book.TotalSentenceCount
                   |> addParameter "$uwc" book.UniqueWordsCount
                   |> execute
                   
        printfn "Save Book func result: %i" code
            
        cn |> commnd "SELECT last_insert_rowid();" |> toLong
            
let saveSentence sentence cn =
    0