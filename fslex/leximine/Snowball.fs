module leximine.Snowball

open System.Text.RegularExpressions

let private lowercase (word: string) =
    word.ToLower()
    
let vowel = [ "a"; "e"; "i"; "o"; "u"; "y" ]
let doubles = [ "bb"; "dd"; "ff"; "gg"; "mm"; "nn"; "pp"; "rr"; "tt" ]
    
let private removeSuffix (suffix: string) (word: string) =
    if word.EndsWith(suffix) then
        word.Substring(0, word.Length - suffix.Length)
    else
        word
       
let rec private removeSuffixes word suffixes =
    suffixes |> List.fold (fun acc s -> removeSuffix s acc) word
    
let private replaceSuffix index (newSuffix: string) (word: string) =
    let w = word.Substring(0, index)
    $"{w}{newSuffix}"
    
let private testSuffix (word: string) (suffix: string) =
    if word.EndsWith(suffix) then
        (true, word.Length - suffix.Length)
    else
        (false, -1)

let private step_0 word =
    [ "'"; "'s"; "'s'" ] |> removeSuffixes word
    
let private step_1a (word: string) =
    match testSuffix word "sess" with
    | (true, index) -> $"{word.Substring(0, word.Length - index)}ss"
    | _ -> match testSuffix word "ied" with
           | (true, index) -> if index > 1 then replaceSuffix index "i" word else replaceSuffix index "ie" word
           | _ -> match testSuffix word "ies" with
                  | (true, index) -> if index > 1 then replaceSuffix index "i" word else replaceSuffix index "ie" word
                  | _ -> match (testSuffix word "us", testSuffix word "ss") with
                         | ((a, _), (b, _)) when a || b -> word
                         | _ -> match testSuffix word "s" with
                                | _ -> word
                                | _ -> word

let snowballEn word =
    word
        |> lowercase
        |> step_0
        |> step_1a