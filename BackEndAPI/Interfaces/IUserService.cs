using System.Collections.Generic;
using System.Threading.Tasks;
using BackEndAPI.Entities;
using BackEndAPI.Models;

namespace BackEndAPI.Interfaces
{
    public interface IUserService
    {
        GetUsersListPagedResponseDTO GetUsers(PaginationParameters paginationParameters);        
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        IEnumerable<User> GetAll();
        
        Task<User> Create(CreateUserModel user);
        Task Update(int id, EditUserModel model);
        Task Disable(int id);

         
    }
}