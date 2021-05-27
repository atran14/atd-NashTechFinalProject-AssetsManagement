import { Assignment } from './Assignment'
import { User } from './User'

export type PaginationParameters = {
  PageNumber: number;
  PageSize: number;
}

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
export interface AssignmentPagedListResponse extends PagedListResponse<Assignment> {}
