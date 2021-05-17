using System.Threading.Tasks;
using BackEndAPI.Entities;
using BackEndAPI.Models;

namespace BackEndAPI.Interfaces
{
    public interface IUserService
    {
        Task Update(int id, EditUserModel model);
        Task Disable(int id);

        Task<User> Create(CreateUserModel user);
         
    }
}