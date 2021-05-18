import { CreateUserModel, User } from "../models/user";
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

    public create = (user: CreateUserModel) => this.instance.post("/api/users", user);

}
