import {} from 'module'

export type UserLogin = {
  username: string
  password: string
}

export interface User {
  id: number
  firstName: string;
  lastName: string;
  dateOfBirth: Date;
  joinedDate: Date;
  gender: number;
  type: number;
  staffCode: string;
  userName: string;
  location: number;
  status: number;
}

export interface LoggedInUser extends User {
  token: string;
}

export enum UserGender {
  MALE,
  FEMALE
}

export enum UserType {
  ADMIN, USER
}

export enum UserStatus {
  ACTIVE,
  DISABLED
}

export enum Location {
  HANOI,
  HOCHIMINH
}
