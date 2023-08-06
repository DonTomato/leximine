module leximine.Db.DbWord

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

let saveWord (word: DbWord) cn =
    let code = cn
               |> command @$"
                    INSERT INTO word (word_id, total_count)
                    VALUES ($word, $count)"
               |> addParameter "$id" word.WordId
               |> addParameter "$count" word.TotalCount
               |> execute
               
    printfn "Save Word func result: %i" code
    
let saveWordForm (word: DbWordForm) cn =
    let code = cn
               |> command @"
                    INSERT INTO word_form (id, word_id, count)
                    VALUES ($id, $word_id, $count)"
               |> addParameter "$id" word.Word
               |> addParameter "$word_id" word.WordId
               |> addParameter "$count" word.Count
               |> execute
               
    printfn "Save Word Form func result: %i" code