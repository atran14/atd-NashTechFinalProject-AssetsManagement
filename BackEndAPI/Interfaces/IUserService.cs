using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using BackEndAPI.Entities;
using BackEndAPI.Models;

namespace BackEndAPI.Interfaces
{
    public interface IUserService
    {
        GetUsersListPagedResponseDTO GetUsers(PaginationParameters paginationParameters);        
        Task Update(int id, EditUserModel model);
        Task Disable(int id);

        Task<User> Create(CreateUserModel user);
         
    }
}