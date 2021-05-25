using System.Threading.Tasks;
using BackEndAPI.Interfaces;
using BackEndAPI.Models;

namespace BackEndAPI.Services
{
    public class ReturnRequestService : IReturnRequestService
    {

        private readonly IAsyncReturnRequestRepository _repository;

        public ReturnRequestService(IAsyncReturnRequestRepository repository) {
            _repository = repository;
        }

        public Task<GetUsersListPagedResponseDTO> FilterReturnRequests(PaginationParameters paginationParameters, int adminId, ReturnRequestFilterParameters type)
        {
            throw new System.NotImplementedException();
        }

        public Task<GetUsersListPagedResponseDTO> GetAllReturnRequests(PaginationParameters paginationParameters, int adminId)
        {
            throw new System.NotImplementedException();
        }

        public Task<GetUsersListPagedResponseDTO> SearchReturnRequests(PaginationParameters paginationParameters, int adminId, string searchQuery)
        {
            throw new System.NotImplementedException();
        }
    }
}