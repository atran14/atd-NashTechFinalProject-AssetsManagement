using System.Threading.Tasks;
using BackEndAPI.Models;

namespace BackEndAPI.Interfaces
{
    public interface IAsyncUserRepository : IAsyncRepository<User>
    {

        bool CheckUsernameExist(string username);

    }
}