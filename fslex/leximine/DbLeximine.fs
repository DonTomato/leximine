module leximine.DbLeximine

open leximine.db

type DbBook = {
    Title: string
    Author: string
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

let saveBook (book: DbBook) cn =
        let code = cn
                   |> command @"
                        INSERT INTO book (title, author, total_words_count, sentence_count, unique_words_count)
                        VALUES ($title, $author, $wc, $sc, $uwc)"
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
               |> command @$"
                    INSERT INTO sentence (sentence, hash, book_id, words_count)
                    VALUES ($sentence, $hash, $bid, $wc)"
               |> addParameter "$sentence" sentence.Sentence
               |> addParameter "$hash" sentence.Hash
               |> addParameter "$bid" sentence.BookId
               |> addParameter "$wc" sentence.WordsCount
               |> execute
               
    printfn "Save Sentence func result: %i" code
    
    cn |> getLastID