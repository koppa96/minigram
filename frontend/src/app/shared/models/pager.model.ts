export interface PagerModel {
  pageIndex: number
  pageSize: number
  totalPages: number
  totalCount: number
}

export interface PagedList<T> {
  pageIndex: number
  pageSize: number
  totalCount: number
  data: T[]
}
