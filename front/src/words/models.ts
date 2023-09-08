export interface InitResponse {
    pageSize: number;
    initialPageIndex: number;
    totalCount: number;
    knownCount: number;
    totalPages: number;
}

export interface WordsPageRecord {
    no: number;
    word: string;
    totalCount: number;
    defaultForm: string;
    known: boolean;
}
