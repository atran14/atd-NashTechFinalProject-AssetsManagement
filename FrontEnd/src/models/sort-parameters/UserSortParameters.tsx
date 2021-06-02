export enum SortOrder {
  Ascend,
  Descend,
}

export enum UserSortColumn {
  StaffCode,
  FullName,
  UserName,
  JoinedDate,
  Type,
}

export interface UserSortParameters {
  column? : UserSortColumn;
  order? : SortOrder;
}
