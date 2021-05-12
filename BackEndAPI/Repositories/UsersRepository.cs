using System.Linq;
using System.Threading.Tasks;
using BackEndAPI.Models;
using BackEndAPI.Interfaces;

namespace BackEndAPI.Repositories
{
    public class UsersRepository : AsyncGenericRepository<User>, IAsyncUserRepository
    {
        public UsersRepository() : base()
        {

        }
    }
}