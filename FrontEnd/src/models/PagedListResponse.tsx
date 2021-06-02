import { Asset } from './Asset';
import { User } from './User'

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

export interface AssetsPagedListResponse extends PagedListResponse<Asset> {}