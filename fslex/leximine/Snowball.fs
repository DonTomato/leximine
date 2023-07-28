module leximine.Snowball

let private lowercase (word: string) =
    word.ToLower()
    
let private removeSuffix (suffix: string) (word: string) =
    if word.EndsWith(suffix) then
        word.Substring(0, word.Length - suffix.Length)
    else
        word
       
let rec private removeSuffixes word suffixes =
    suffixes |> List.fold (fun acc s -> removeSuffix s acc) word

let private step_0 word =
    [ "'"; "'s"; "'s'" ] |> removeSuffixes word
    
let private step_1b word =
    0

let snowballEn text =
    text
        |> lowercase
        |> step_0