using System.Threading.Tasks;
using BackEndAPI.Models;

namespace BackEndAPI.Interfaces
{
    public interface IReturnRequestService
    {
        Task<GetUsersListPagedResponseDTO> GetAllReturnRequests(
           PaginationParameters paginationParameters,
           int adminId
       );

        Task<GetUsersListPagedResponseDTO> FilterReturnRequests(
            PaginationParameters paginationParameters,
            int adminId,
            ReturnRequestFilterParameters type
        );

        Task<GetUsersListPagedResponseDTO> SearchReturnRequests(
            PaginationParameters paginationParameters,
            int adminId,
            string searchQuery
        );   
    }
}