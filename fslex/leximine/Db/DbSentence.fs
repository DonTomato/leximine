module leximine.DbLeximine.Sentence

open leximine.db

type DbSentence = {
    Id: string
    Sentence: string
    WordsCount: int
}

let saveSentence sentence cn =
    let code = cn
               |> command @$"
                    INSERT INTO sentence (id, sentence, words_count)
                    VALUES ($id, $sentence, $wc)"
               |> addParameter "id" sentence.Id
               |> addParameter "$sentence" sentence.Sentence
               |> addParameter "$wc" sentence.WordsCount
               |> execute
               
    printfn "Save Sentence func result: %i" code