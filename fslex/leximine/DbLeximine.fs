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

let private getStrLang lang =
    match lang with
    | English -> "en"
    | Norwegian -> "no"

let saveBook book cn =
        cn
            |> leximine.db.commnd @"
                INSERT INTO book (lang, title, author, words_count, sentence_count, unique_words_count)
                VALUES ($lang, $title, $author, $wc, $sc, $uwc)"
            |> leximine.db.addParameter "$lang" (getStrLang book.Lang)
            |> leximine.db.addParameter "$title" book.Title
            |> leximine.db.addParameter "$author" book.Author
            |> leximine.db.addParameter "$wc" book.TotalWordsCount
            |> leximine.db.addParameter "$sc" book.TotalSentenceCount
            |> leximine.db.addParameter "$uwc" book.UniqueWordsCount
            |> leximine.db.execute
            
let saveSentence sentence cn =
    0