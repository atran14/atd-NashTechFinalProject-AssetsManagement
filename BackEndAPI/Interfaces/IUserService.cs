using System.Linq;
using System.Collections.Generic;
using BackEndAPI.Models;

namespace BackEndAPI.Interfaces
{
    public interface IUserService
    {
        GetUsersListPagedResponseDTO GetUsers(PaginationParameters paginationParameters);        
    }
}