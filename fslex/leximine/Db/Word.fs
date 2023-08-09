module leximine.Db.Word

open leximine.db

type DbWord = {
    WordId: string
    TotalCount: int
}

type DbWordForm = {
    Word: string
    WordId: string
    Count: int
}

type DbBookWord = {
    WordId: string
    Count: int
}

type DbBookWordForm = {
    WordForm: string
    Count: int
}

let readAllWordId cn =
    cn
    |> command @"SELECT word_id FROM word"
    |> query (fun read -> read.string "word_id")
    
let readAllWordForms cn =
    cn
    |> command "SELECT id FROM word_form"
    |> query (fun read -> read.string "id")

let saveWord (word: DbWord) cn =
    cn
    |> command @$"
        INSERT INTO word (word_id, total_count)
        VALUES ($word, $count)"
    |> addParameter "$word" word.WordId
    |> addParameter "$count" word.TotalCount
    |> execute
    
let saveWordForm (word: DbWordForm) cn =
    cn
    |> command @"
        INSERT INTO word_form (id, word_id, count)
        VALUES ($id, $word_id, $count)"
    |> addParameter "$id" word.Word
    |> addParameter "$word_id" word.WordId
    |> addParameter "$count" word.Count
    |> execute
    
let saveBookWord (word: DbBookWord) (bookId: int64) cn =
    cn
    |> command @"
        INSERT INTO book_word (book_id, word_id, count)
        VALUES ($book_id, $word_id, $count)"
    |> addParameter "$book_id" bookId
    |> addParameter "$word_id" word.WordId
    |> addParameter "$count" word.Count
    |> execute
    
let saveBookWordForm (word: DbBookWordForm) (bookId: int) cn =
    cn
    |> command @"
        INSERT INTO book_word_form (book_id, word_form_id, count)
        VALUES ($book_id, $word_form, $count)"
    |> addParameter "$book_id" bookId
    |> addParameter "$word_form" word.WordForm
    |> addParameter "$count" word.Count
    |> execute