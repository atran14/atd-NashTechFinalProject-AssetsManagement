import { UserType } from "./User";

interface GenericSearchFilterParameters {
    searchQuery: string | "";
}

export interface UserSearchFilterParameters extends GenericSearchFilterParameters {
    type?: UserType;
}