import { User } from './user'

interface PagedListResponse<TModel> {
  totalCount: number
  pageSize: number
  currentPage: number
  totalPages: number
  hasNext: boolean
  hasPrevious: boolean
  items: TModel[]
}

export interface UsersPagedListResponse extends PagedListResponse<User> {}
