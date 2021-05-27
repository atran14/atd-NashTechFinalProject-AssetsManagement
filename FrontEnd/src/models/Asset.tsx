import {} from "module";

export type Asset = {
  id: number;
  assetCode: string;
  assetName: string;
  categoryId: number;
  category: Category;
  specification: string;
  installedDate: Date;
  state: number;
  location: number;
};
export enum Location {
  HANOI,
  HOCHIMINH,
}

export enum AssetState {
    Available,
    NotAvailable,
    WaitingForRecycling,
    Recycled
}

export type Category = {
  id: number;
  categoryCode : string;
  categoryName: string;
};
