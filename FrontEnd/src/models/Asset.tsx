import { AssetCategory } from "./AssetCategory"

export type AssetInfo = {

    assetName: string,

    categoryId: number,

    specification: string,

    installedDate: Date,

    state: number,

    location: number

}

// export type EditUserModel = {
//     dateOfBirth: Date,
//     joinedDate: Date,
//     gender: number,
//     type: number
// }

export interface Asset {

    id:number,

    assetName: string,

    assetCode:string,

    categoryId: number,
    category: AssetCategory,

    specification: string,

    installedDate: Date,

    state: number,

    location: number

}

export type CreateAssetModel = {

    assetName: string,

    categoryId: number,

    specification: string,

    installedDate: Date,

    state: number,

    location: number

}


export enum AssetState {

    AVAILABLE,

    NOTAVAILABLE,
    
    ASSIGNED,

    WAITINGFORRECYCLING,

    RECYCLED
    
}

export enum Location {

    HANOI,

    HOCHIMINH

}
