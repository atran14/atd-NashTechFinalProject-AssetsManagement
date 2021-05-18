import { User, UserLogin } from "../models/user";
import { HttpClient } from "./HttpClient";

export class AuthenticationSerivce extends HttpClient{
  private static classInstance?: AuthenticationSerivce;

  public constructor() {
    super('https://localhost:5001');
  }

  public static getInstance() {
    if (!this.classInstance) {
      this.classInstance = new AuthenticationSerivce();
    }

    return this.classInstance;
  }

  public login = (user: UserLogin) => this.instance.post<User>("/login", user);

  public logout = ()=>this.instance.post("/logout");
}
