using System.Linq;
using System.Collections.Generic;
using BackEndAPI.Models;

namespace BackEndAPI.Interfaces
{
    public interface IUserService
    {
        IQueryable<User> GetAllUsers();
        
    }
}