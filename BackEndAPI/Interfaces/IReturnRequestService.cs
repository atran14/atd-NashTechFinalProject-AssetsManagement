using System.Threading.Tasks;
using BackEndAPI.Models;

namespace BackEndAPI.Interfaces
{
    public interface IReturnRequestService
    {
        Task<GetReturnRequestsPagedResponseDTO> GetAllReturnRequests(
           PaginationParameters paginationParameters,
           int adminId
       );

        Task<GetReturnRequestsPagedResponseDTO> FilterReturnRequests(
            PaginationParameters paginationParameters,
            int adminId,
            ReturnRequestFilterParameters filterParameters
        );

        Task<GetReturnRequestsPagedResponseDTO> SearchReturnRequests(
            PaginationParameters paginationParameters,
            int adminId,
            string searchQuery
        );   
    }
}