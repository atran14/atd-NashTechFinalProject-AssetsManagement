import { UsersPagedListResponse } from "../models/PagedListResponse";
import { HttpClient } from "./HttpClient";

export class UserService extends HttpClient {
    private static classInstance?: UserService;

    private constructor() {
        super('https://localhost:5001');
    }

    public static getInstance = (): UserService => {
        if (UserService.classInstance === undefined) {
            UserService.classInstance = new UserService();
        }

        return UserService.classInstance;
    }

    public getUsers = () => this.instance.get<UsersPagedListResponse>("/api/Users");

}
