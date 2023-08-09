module leximine.BookStatisticSaver

open leximine.Helpers
open leximine.Book

let processBook fileName dbPath (stem: StemFn) =
    
    let dbFileName = System.IO.Path.Combine(dbPath, "leximine_dev.db")
    
    use cn = db.createCn dbFileName
    
    let transaction = cn |> db.transaction
    
    let data = Fb2Parser.load fileName
    let bookData = Fb2Parser.parseData data
    
    printfn "Book of %s: %s" bookData.Author bookData.Title
    log "Book paragraphs" (bookData.Paragraphs |> List.length)
    
    // Get sentence statistics
    let sentenceStat = SentenceParser.parseBookToSentenceStatistic bookData.Paragraphs stem
    
    let sentenceTotalCountInBook = sentenceStat |> List.length
    let wordTotalCountInBook = sentenceStat
                               |> List.map (fun s -> s.WordCount)
                               |> List.sum                     
    let uniqueWordCountInBook = sentenceStat
                                |> List.map (fun s -> s.WordForms)
                                |> List.concat
                                |> List.map (fun s -> s.WordID)
                                |> List.distinct
                                |> List.length
    
    log "Total sentences" sentenceTotalCountInBook
    log "Total words" wordTotalCountInBook
    log "Unique words" uniqueWordCountInBook
    
    // Save book
    let bookId = cn |> Db.Book.saveBook {
        Title = bookData.Title
        Author = bookData.Author
        TotalSentenceCount = sentenceTotalCountInBook
        TotalWordsCount = wordTotalCountInBook
        UniqueWordsCount = uniqueWordCountInBook
    }
    
    // Parse words
    
    let existingWords = cn |> Db.Word.readAllWordId
    
    let newWords = sentenceStat |> SentenceParser.getNewWords existingWords stem
    
    newWords
    |> List.iter (fun w ->
        cn
        |> Db.Word.saveWord {
            WordId = w
            TotalCount = 0
        }
        |> ignore)
    
    log "New Words" (newWords |> List.length)
    
    // Parse words forms
    
    let existingWordForms = cn |> Db.Word.readAllWordForms
    
    let newWordForms = sentenceStat |> SentenceParser.getNewWordForms existingWordForms
    newWordForms |> List.iter (fun w ->
        cn
        |> Db.Word.saveWordForm {
            WordId = w.WordID
            Word = w.WordForm
            Count = 0 
        }
        |> ignore)
    
    log "New Word Forms" (newWordForms |> List.length)
    
    let existingSentenceHashes = cn |> Db.Sentence.readAllSentenceHashes
    
    let newSentences = sentenceStat
                       |> SentenceParser.getNewSentences existingSentenceHashes
    
    newSentences |> List.iter (fun s ->
        cn
        |> Db.Sentence.saveSentence {
            Id = s.Hash
            Sentence = s.Sentence
            WordsCount = s.WordCount 
        }
        |> ignore)
    
    log "New Sentences" (newSentences |> List.length)
    
    let bookSentences = sentenceStat
                        |> List.groupBy (fun s -> s.Hash)
                        |> List.map (fun (key, values) -> {
                            Db.Sentence.DbBookSentence.SentenceID = key;
                            Db.Sentence.DbBookSentence.BookID = bookId
                            Db.Sentence.DbBookSentence.Count = values |> List.length
                        })
    bookSentences
    |> List.iter (fun s ->
        cn
        |> Db.Sentence.saveBookSentence s
        |> ignore)
    
    log "Unique Book Sentences" (bookSentences |> List.length)
    
    let bookWords = sentenceStat |> SentenceParser.getBookWords
    
    bookWords
    |> List.map (fun (w, c) -> {
        Db.Word.DbBookWord.WordId = w
        Db.Word.DbBookWord.Count = c 
    })
    |> List.iter (fun wb ->
        cn
        |> Db.Word.saveBookWord wb bookId
        |> ignore)
    
    log "Word for this book" (bookWords |> List.length)
    
    transaction |> db.commit