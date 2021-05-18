import { CreateUserModel, User, UserInfo, UserInput } from "../models/user";
import { HttpClient } from "./HttpClient";

export class UserSerivce extends HttpClient {
    private static classInstance?: UserSerivce;

    private constructor() {
        super('https://localhost:5001');
    }

    public static getInstance() {
        if (!this.classInstance) {
            this.classInstance = new UserSerivce();
        }

        return this.classInstance;
    }

    public create = (user: CreateUserModel) => this.instance.post<User>("/api/users", user);
    
    public getUser = (id: number) => this.instance.get<UserInfo>(`/api/Users/${id}`);

    public updateUser = (user: UserInput, id: number) => this.instance.put<UserInput>(`/api/Users/${id}`, user);

}
