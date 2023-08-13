module leximine.Db.WordSentence

open leximine.db

type WordFormSentence = {
    WordForm: string
    SentenceID: string
}

let readAllExistingWordFormSentenceLinks cn =
    cn
    |> command "SELECT word_form_id, sentence_id FROM word_form_sentence"
    |> query (fun read -> (read.string "word_form_id", read.string "sentence_id"))
    
let readAllExistingWordSentenceLinks cn =
    cn
    |> command "SELECT word_id, sentence_id FROM word_sentence"
    |> query (fun read -> (read.string "word_id", read.string "sentence_id"))

let saveWordFormSentence w cn =
    cn
    |> command @"
        INSERT INTO word_form_sentence (word_form_id, sentence_id)
        VALUES ($w, $sid)"
    |> addParameter "$w" w.WordForm
    |> addParameter "$sid" w.SentenceID
    |> execute
    
    
type WordSentence = {
    WordID: string
    SentenceID: string
}

let saveWordSentence ws cn =
    cn
    |> command @"
        INSERT INTO word_sentence (word_id, sentence_id)
        VALUES ($wid, $sid)"
    |> addParameter "$wid" ws.WordID
    |> addParameter "$sid" ws.SentenceID
    |> execute