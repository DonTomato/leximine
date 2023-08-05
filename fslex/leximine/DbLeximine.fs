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
    BookId: int64
}

let private getStrLang lang =
    match lang with
    | English -> "en"
    | Norwegian -> "no"

let saveBook (book: DbBook) cn =
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
            
        cn |> getLastID
            
let saveSentence sentence cn =
    let code = cn
               |> commnd @$"
                    INSERT INTO sentence (sentence, hash, book_id, words_count)
                    VALUES ($sentence, $hash, $bid, $wc)"
               |> addParameter "$sentence" sentence.Sentence
               |> addParameter "$hash" sentence.Hash
               |> addParameter "$bid" sentence.BookId
               |> addParameter "$wc" sentence.WordsCount
               |> execute
               
    printfn "Save Sentence func result: %i" code
    
    cn |> getLastID