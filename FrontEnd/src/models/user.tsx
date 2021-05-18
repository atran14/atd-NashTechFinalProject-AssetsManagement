import { } from "module";

export type UserInfo = {
  firstName: string,
  lastName : string,
  dateOfBirth: Date,
  joinedDate: Date,
  gender: number,
  type : number
}

export type EditUserModel  = {
  dateOfBirth: Date,
  joinedDate: Date,
  gender: number,
  type : number
}


export type UserOnRegister = {
  username: string,
  password: string,
  passwordConfirm: string
}

export type UserLogin = {
  username: string,
  password: string
}

export type User = {
  id: number,
  username: string,
  password: string,
  role: {
    name: string
  }
}
