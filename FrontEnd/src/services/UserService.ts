
import { User, UserInfo, UserInput, UserOnRegister } from "../models/user";
import { HttpClient } from "./HttpClient";

export class UserService extends HttpClient {

  public constructor() {
    super("https://localhost:5001");
  }

  public register = (user: UserOnRegister) => this.instance.post<User>("/register", user);
  public getUser = (id: number) => this.instance.get<UserInfo>(`/api/Users/${id}`);

  public  updateUser = (user: UserInput, id: number) => this.instance.put<UserInput>(`/api/Users/${id}`, user);

}