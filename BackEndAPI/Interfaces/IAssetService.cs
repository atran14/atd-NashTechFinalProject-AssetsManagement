using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEndAPI.Entities;
using BackEndAPI.Enums;
using BackEndAPI.Models;

namespace BackEndAPI.Interfaces
{
    public interface IAssetService
    {

        Task<GetUsersListPagedResponseDTO> GetUsers(
            PaginationParameters paginationParameters,
            int adminId
        );

        Task<Asset> Create(CreateAssetModel model);

    }
}