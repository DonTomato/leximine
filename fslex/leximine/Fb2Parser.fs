module leximine.Fb2Parser

open System.Xml.Linq

let parseData (data: XDocument) =
    let ns = data.Root.GetDefaultNamespace()
    
    let getTextFromNode (node: XElement) = node.Value
    
    let getChildNode nodeName (node: XElement) =
        node.Elements(ns + nodeName) |> Seq.item 0
        
    let getChildValue nodeName (node: XElement) =
        node |> getChildNode nodeName |> getTextFromNode
    
    let description = data.Root |> getChildNode "description"
    let titleInfo = description |> getChildNode "title-info"
    let author = titleInfo |> getChildNode "author"
    {
        Book.BookData.Title = titleInfo |> getChildValue "book-title";
        Book.BookData.Author = (author |> getChildValue "first-name", author |> getChildValue "last-name")
            |> fun (fn, ln) -> $"%s{fn} %s{ln}";
        Book.BookData.Sections = [];
    }
    