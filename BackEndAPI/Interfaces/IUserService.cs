using System.Collections.Generic;
using System.Threading.Tasks;
using BackEndAPI.Entities;
using BackEndAPI.Enums;
using BackEndAPI.Models;

namespace BackEndAPI.Interfaces
{
    public interface IUserService
    {
        Task<GetUsersListPagedResponseDTO> GetUsers(
            PaginationParameters paginationParameters,
            int adminId
        );

        Task<GetUsersListPagedResponseDTO> GetUsers(
            PaginationParameters paginationParameters,
            int adminId,
            UserType type
        );

        Task<GetUsersListPagedResponseDTO> SearchUsersByFullName(
            PaginationParameters paginationParameters,
            int adminId,
            string searchText
        );

        Task<GetUsersListPagedResponseDTO> SearchUsersByStaffCode(
            PaginationParameters paginationParameters,
            int adminId,
            string searchText
        );

        AuthenticateResponse Authenticate(AuthenticateRequest model);
        IEnumerable<User> GetAll();

        Task<User> Create(CreateUserModel user);
        Task Update(int id, EditUserModel model);
        Task Disable(int id);
        Task<UserInfo> GetById(int id);


    }
}