export interface InitResponse {
    pageSize: number;
    initialPageIndex: number;
    totalCount: number;
    knownCount: number;
    totalPages: number;
}

export interface WordsPageRecord {
    word: string;
    totalCount: number;
    defaultForm: string;
    known: boolean;
}
