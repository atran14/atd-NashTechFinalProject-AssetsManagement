using System.Threading.Tasks;
using BackEndAPI.Models;

namespace BackEndAPI.Interfaces
{
    public interface IUserService
    {
        Task Update(int id, EditModel model);
         
    }
}