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
    let sentenceStat = SentenceParser.parseBookToSentenceStatisic bookData.Paragraphs
    
    let sentenceTotalCountInBook = sentenceStat |> List.length
    let wordTotalCountInBook = sentenceStat
                               |> List.map (fun s -> s.WordCount)
                               |> List.sum                     
    let uniqueWordCountInBook = sentenceStat
                                |> List.map (fun s -> s.WordForms)
                                |> List.concat
                                |> List.map (fun s -> stem s)
                                |> List.distinct
                                |> List.length
    
    log "Total sentences" sentenceTotalCountInBook
    log "Total words" wordTotalCountInBook
    log "Unique words" uniqueWordCountInBook
    
    // Save book
    let bookId = cn |> DbLeximine.saveBook {
        Title = bookData.Title
        Author = bookData.Author
        TotalSentenceCount = sentenceTotalCountInBook
        TotalWordsCount = wordTotalCountInBook
        UniqueWordsCount = uniqueWordCountInBook 
    }
    
    // Parse words
    
    // let existingWords = DbLeximine.Book.
    
    transaction |> db.commit