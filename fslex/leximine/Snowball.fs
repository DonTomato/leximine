module leximine.Snowball

open System.Text.RegularExpressions

let private lowercase (word: string) =
    word.ToLower()
    
let private removeSuffix (suffix: string) (word: string) =
    if word.EndsWith(suffix) then
        word.Substring(0, word.Length - suffix.Length)
    else
        word
       
let rec private removeSuffixes word suffixes =
    suffixes |> List.fold (fun acc s -> removeSuffix s acc) word

let testSuffix (word: string) (suffixPattern: string) =
    let m = Regex($"{suffixPattern}$").Match(word)
    match m with
    | m when m.Success -> (true, m.Index)
    | _ -> (false, -1)

let private step_0 word =
    [ "'"; "'s"; "'s'" ] |> removeSuffixes word
    
let private step_1b (word: string) =
    match testSuffix word "sess" with
    | (true, index) -> $"{word.Substring(0, word.Length - index)}ss"
    | _ -> match testSuffix word ""

let snowballEn text =
    text
        |> lowercase
        |> step_0
        |> step_1b