import { PaginationParameters, ReturnRequestPagedListResponse } from "../models/Pagination";
import { CreateReturnRequestModel, ReturnRequestFilterParameters } from "../models/ReturnRequest";
import { HttpClient } from "./HttpClient";

export class ReturnRequestService extends HttpClient {
  private static instance?: ReturnRequestService;

  private constructor() {
    super('https://localhost:5001');
  }

  public static getInstance = (): ReturnRequestService => {
    if (ReturnRequestService.instance === undefined) {
      ReturnRequestService.instance = new ReturnRequestService();
    }

    return ReturnRequestService.instance;
  }

  public getAll = (parameters?: PaginationParameters) => this.instance.get<ReturnRequestPagedListResponse>(
    "/api/ReturnRequests",
    {
      params: {
        PageNumber: parameters?.PageNumber ?? 1,
        PageSize: parameters?.PageSize ?? 10
      }
    });

  public search = (searchText: string, parameters?: PaginationParameters) => 
    this.instance.get<ReturnRequestPagedListResponse>(
      `/api/ReturnRequests/search`,
      {
        params: {
          query: searchText,
          PageNumber: parameters?.PageNumber ?? 1,
          PageSize: parameters?.PageSize ?? 10
        }
      }
    )

  public filter = (filter?: ReturnRequestFilterParameters, parameters?: PaginationParameters) =>
    this.instance.get<ReturnRequestPagedListResponse>(
      `api/ReturnRequests/filter`,
      {
        params: {
          ReturnedDate: filter?.returnedDate,
          RequestStateL: filter?.state,
          PageNumber: parameters?.PageNumber ?? 1,
          PageSize: parameters?.PageSize ?? 10
        }
      }
    )

  public create = (parameters: CreateReturnRequestModel) =>
      this.instance.post(`/api/ReturnRequests`, parameters);
}
