module leximine.Db.Book

open leximine.db

type DbBook = {
    Title: string
    Author: string
    TotalWordsCount: int
    TotalSentenceCount: int
    UniqueWordsCount: int
}

let saveBook (book: DbBook) cn =
    cn
    |> command @"
        INSERT INTO book (title, author, total_words_count, sentence_count, unique_words_count)
        VALUES ($title, $author, $wc, $sc, $uwc)"
    |> addParameter "$title" book.Title
    |> addParameter "$author" book.Author
    |> addParameter "$wc" book.TotalWordsCount
    |> addParameter "$sc" book.TotalSentenceCount
    |> addParameter "$uwc" book.UniqueWordsCount
    |> execute
    |> ignore
        
    cn |> getLastID