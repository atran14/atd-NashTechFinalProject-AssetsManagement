
import { Asset, CreateAssetModel } from "../models/Asset";
import { HttpClient } from "./HttpClient";
export class AssetService extends HttpClient {

  private static instance?: AssetService;

  private constructor() {
    super("https://localhost:5001");
  }

  public static getInstance = (): AssetService => {
    if (AssetService.instance === undefined) {
        AssetService.instance = new AssetService();
    }

    return AssetService.instance;
  }

  public create = (asset: CreateAssetModel) => this.instance.post<Asset>("/api/assets", asset);

  public getAllAssets = () => this.instance.get(`/api/Asset/getallasset/${JSON.parse(sessionStorage.getItem("id")!)}`);

  public getAssetsBySearch = (searchText : string) => this.instance.get(`/api/Asset/search/${JSON.parse(sessionStorage.getItem("id")!)}/${searchText}`);

  public getAsset = (id : number) => this.instance.get<Asset>(`/api/Asset/${id}`);
}
