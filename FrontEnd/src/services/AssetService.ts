
import { Asset, CreateAssetModel } from "../models/Asset";
import { PaginationParameters, UsersPagedListResponse } from "../models/Pagination";
import { UserInfo, EditUserModel, UserType } from "../models/User";
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

  public getUsers = (parameters?: PaginationParameters) => this.instance.get<UsersPagedListResponse>(
    "/api/Users",
    {
      params: {
        PageNumber: parameters?.PageNumber ?? 1,
        PageSize: parameters?.PageSize ?? 10
      }
    });

  public getUser = (id: number) => this.instance.get<UserInfo>(`/api/Users/${id}`);

  public filterByType = (type: UserType, parameters?: PaginationParameters) => {
    return this.instance.get<UsersPagedListResponse>(`/api/Users/type/${type.valueOf()}`,
    {
      params: {
        PageNumber: parameters?.PageNumber ?? 1,
        PageSize: parameters?.PageSize ?? 10
      }
    })
  }

  public searchUsers = (searchText: string, parameters?: PaginationParameters) => this.instance.get<UsersPagedListResponse>(
    `/api/Users/search`,
    {
      params: {
        query: searchText,
        PageNumber: parameters?.PageNumber ?? 1,
        PageSize: parameters?.PageSize ?? 10
      }
    })

  public updateUser = (user: EditUserModel, id: number) => this.instance.put<EditUserModel>(`/api/Users/${id}`, user);

  public disableUser = (id: number) => this.instance.put(`/api/Users/disable/${id}`);
}
