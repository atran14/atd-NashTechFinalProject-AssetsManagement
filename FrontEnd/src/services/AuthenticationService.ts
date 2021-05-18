import { User, UserLogin } from "../models/User";
import { HttpClient } from "./HttpClient";

export class AuthenticationService extends HttpClient{
  private static classInstance?: AuthenticationService;

  private constructor() {
    super('https://localhost:5001');
  }

  public static getInstance = () : AuthenticationService => {
    if (AuthenticationService.classInstance === undefined) {
      AuthenticationService.classInstance = new AuthenticationService();
    }

    return AuthenticationService.classInstance;
  } 

  public login = (user: UserLogin) => this.instance.post<User>("/login", user);

  public logout = ()=>this.instance.post("/logout");
}
