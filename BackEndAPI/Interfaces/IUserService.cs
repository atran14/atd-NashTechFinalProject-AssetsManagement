using System.Threading.Tasks;
using BackEndAPI.Entities;
using BackEndAPI.Models;

namespace BackEndAPI.Interfaces
{
    public interface IUserService
    {
        
        Task<User> Create(CreateUserModel user);
        Task Update(int id, EditUserModel model);
        Task Disable(int id);

         
    }
}