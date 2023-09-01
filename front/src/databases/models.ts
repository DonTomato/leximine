export interface DbInfo {
    language: string;
    fileName: string;
    size: number;
}

export interface DatabaseListResponse {
    mainDb: DbInfo;
    backups: DbInfo[];
}
