module leximine.Logic.FileParsers.Fb2

open System.Xml.Linq
open leximine.Logic.FileParsers.Main

let load (fn: string) =
    XDocument.Load fn

let parseData (data: XDocument) =
    let ns = data.Root.GetDefaultNamespace()

    let getTextFromNode (node: XElement) = node.Value

    let getChildNode nodeName (node: XElement) =
        node.Elements(ns + nodeName) |> Seq.item 0

    let getChildValue nodeName (node: XElement) =
        node |> getChildNode nodeName |> getTextFromNode

    let rec getParagraphs (s: XElement) =
        let pElements = s.Elements(ns + "p") |> Seq.toList
        let sectionElements = s.Elements(ns + "section")

        let ps = pElements |> List.map getTextFromNode

        let psFromChildSections = sectionElements |> Seq.map getParagraphs |> Seq.concat |> Seq.toList

        ps @ psFromChildSections


    let description = data.Root |> getChildNode "description"
    let titleInfo = description |> getChildNode "title-info"
    let author = titleInfo |> getChildNode "author"

    let body = data.Root |> getChildNode "body"

    {
        BookData.Title = titleInfo |> getChildValue "book-title";
        BookData.Author = (author |> getChildValue "first-name", author |> getChildValue "last-name")
            |> fun (fn, ln) -> $"%s{fn} %s{ln}";
        BookData.Sentences = getParagraphs body
                             |> List.map (fun p -> textToSentences p)
                             |> List.concat
    }