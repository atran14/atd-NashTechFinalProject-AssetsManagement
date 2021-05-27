import {} from "module";
import { Asset } from "./Asset";
import {  User } from "./User";

export type AssignmentModel = {
    assetId: number;
    assignedToUserId: number;
    assignedDate: Date;
    note: string;
};

export type AssignmentInfo = {
    assetId: number;
    assignedToUserId: number;
    assignedDate: Date;
    note: string;
    state: number;
};

export type Assignment = {
    id: number;
    assetId: number;
    asset : Asset;
    assignedToUserId: number;
    assignedToUser: User;
    assignedByUserId:number;
    assignedByUser : User;
    assignedDate: Date;
    note: string;
    state: number;
};


export enum AssignmentState
{

    WaitingForAcceptance,

    Accepted,

    Declined
    
}