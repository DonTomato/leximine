module leximine.Book

type BookData = {
    Title: string
    Author: string
    Paragraphs: string list
}

type StemFn = string -> string