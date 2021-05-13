using System.Threading.Tasks;
using BackEndAPI.Models;

namespace BackEndAPI.Interfaces
{
    public interface IUserService
    {
        Task Update(int id, EditUserModel model);
        Task Disable(int id);
         
    }
}