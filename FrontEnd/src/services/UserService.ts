
import { UsersPagedListResponse } from "../models/PagedListResponse";
import { User, UserInfo, EditUserModel, CreateUserModel } from "../models/User";
import { HttpClient } from "./HttpClient";
export class UserService extends HttpClient {

  private static instance?: UserService;

  public constructor() {
    super("https://localhost:5001");
  }

  public static getInstance = (): UserService => {
    if (UserService.instance === undefined) {
      UserService.instance = new UserService();
    }

    return UserService.instance;
  }

  public create = (user: CreateUserModel) => this.instance.post<User>("/api/users", user);

  public getUsers = () => this.instance.get<UsersPagedListResponse>("/api/Users");

  public getUser = (id: number) => this.instance.get<UserInfo>(`/api/Users/${id}`);

  public updateUser = (user: EditUserModel, id: number) => this.instance.put<EditUserModel>(`/api/Users/${id}`, user);

  public disableUser = (id: number) => this.instance.put(`/api/Users/disable/${id}`);
}
