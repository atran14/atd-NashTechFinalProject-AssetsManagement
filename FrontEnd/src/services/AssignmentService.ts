import moment from "moment";
import { Assignment, AssignmentInfo, AssignmentModel, AssignmentState } from "../models/Assignment";
import { AssignmentPagedListResponse, PaginationParameters } from "../models/Pagination";
import { HttpClient } from "./HttpClient";

export class AssignmentsService extends HttpClient {

    private static instance?: AssignmentsService;
  
    private constructor() {
      super("https://localhost:5001");
    }
  
    public static getInstance = (): AssignmentsService => {
      if (AssignmentsService.instance === undefined) {
        AssignmentsService.instance = new AssignmentsService();
      }
  
      return AssignmentsService.instance;
    }

    public getAssignments = (parameters?: PaginationParameters) => this.instance.get<AssignmentPagedListResponse>(
      "/api/Assignments",
      {
        params: {
          PageNumber: parameters?.PageNumber ?? 1,
          PageSize: parameters?.PageSize ?? 10
        }
      });

    public create = (assignment : AssignmentModel) => this.instance.post(`/api/Assignments/${JSON.parse(sessionStorage.getItem("id")!)}`, assignment);
    public update = (id : number ,assignment : AssignmentModel) => this.instance.put(`/api/Assignments/${id}`, assignment);
    public getAssignment = (id : number) => this.instance.get<Assignment>(`/api/Assignments/${id}`);
    public getAllNoCondition = () => this.instance.get("/api/Assignments/getAllNoCondition");
    public filterByState = (state : AssignmentState, parameters?: PaginationParameters) => {
      return this.instance.get<AssignmentPagedListResponse>(`/api/Assignments/state/${state.valueOf()}`,
      {
        params: {
          PageNumber: parameters?.PageNumber ?? 1,
          PageSize: parameters?.PageSize ?? 10
        }
      })
    }

    public filterByDate = (date : Date, parameters?: PaginationParameters) => {
      return this.instance.get<AssignmentPagedListResponse>(`/api/Assignments/state/${date}`,
      {
        params: {
          PageNumber: parameters?.PageNumber ?? 1,
          PageSize: parameters?.PageSize ?? 10
        }
      })
    }
  
    public searchAssignment = (searchText: string, parameters?: PaginationParameters) => this.instance.get<AssignmentPagedListResponse>(
      `/api/Assignments/search`,
      {
        params: {
          query: searchText,
          PageNumber: parameters?.PageNumber ?? 1,
          PageSize: parameters?.PageSize ?? 10
        }
      })
}


    
  