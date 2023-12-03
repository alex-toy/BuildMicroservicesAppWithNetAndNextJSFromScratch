export type PagedResult<T> = {
    results: T[]
    pageCount: number
    totalCount: number
}
