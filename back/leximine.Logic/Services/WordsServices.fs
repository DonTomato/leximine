module leximine.Logic.Services.WordsServices

open leximine.SqliteDb
open leximine.Logic.Helpers.DbFileHelper

let private pageSize = 10

type InitResponse = {
    PageSize: int
    InitialPageIndex: int
    TotalCount: int
    KnownCount: int
    TotalPages: int
}

type WordsPageResponse = {
    Word: string
    TotalCount: int
    DefaultForm: string
    Known: bool
}

type WordFilterType =
    | All of int
    | Known of int
    | Unknown of int

let getInitData ln =
    let cn = createCn (getCurrentDbFileName ln)
    let totalCount = cn
                     |> command "SELECT count(*) FROM words"
                     |> queryScalar toInt

    let knownCount = cn
                     |> command "SELECT count(*) FROM words WHERE known = 1"
                     |> queryScalar toInt

    let rowNumberOfFirstUnknown = cn
                                  |> command @"
                                        WITH WordRows AS (
                                            SELECT known, total_count, ROW_NUMBER() OVER (ORDER BY total_count DESC) AS RowNumber
                                            FROM word
                                        )
                                        SELECT COALESCE(
                                            (SELECT RowNumber
                                             FROM WordRows
                                             WHERE known = 1
                                             LIMIT 1),
                                            1
                                        ) as result;"
                                  |> queryScalar toInt

    let firstIndex = match rowNumberOfFirstUnknown with
                     | 0 -> 0
                     | _ -> rowNumberOfFirstUnknown - 1

    let initPageIndex = firstIndex / pageSize

    {
        PageSize = pageSize
        TotalCount = totalCount
        TotalPages = (totalCount / pageSize) + 1
        InitialPageIndex = initPageIndex
        KnownCount = knownCount
    }

let getWordsPage ln filter =
    let pageIndex = match filter with
                    | All i -> i
                    | Known i -> i
                    | Unknown i -> i

    let cn = createCn (getCurrentDbFileName ln)

    let filterClause = match filter with
                       | All _ -> ""
                       | Known _ -> "WHERE known = 1"
                       | Unknown _ -> "WHERE known = 0"

    cn
    |> command $@"
        SELECT word_id, total_count, default_form, known
        FROM word
        {filterClause}
        ORDER BY total_count DESC
        LIMIT $pageSize OFFSET $offsetIndex"

    |> addParameter "$pageSize" pageSize
    |> addParameter "$offsetIndex" (pageIndex * pageSize)
    |> query (fun read -> {
        Word = read.string "word_id"
        TotalCount = read.int "total_count"
        DefaultForm = read.string "default_form"
        Known = read.int "known" = 1
    })