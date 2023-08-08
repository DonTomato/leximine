module leximine.Db.Sentence

open leximine.db

type DbSentence = {
    Id: string
    Sentence: string
    WordsCount: int
}

let readAllSentenceHashes cn =
    cn
    |> command "SELECT id FROM sentence"
    |> query (fun read -> read.string "id")

let saveSentence sentence cn =
    cn
    |> command @$"
        INSERT INTO sentence (id, sentence, words_count)
        VALUES ($id, $sentence, $wc)"
    |> addParameter "id" sentence.Id
    |> addParameter "$sentence" sentence.Sentence
    |> addParameter "$wc" sentence.WordsCount
    |> execute