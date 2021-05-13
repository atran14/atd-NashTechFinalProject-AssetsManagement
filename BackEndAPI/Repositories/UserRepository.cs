using System.Linq;
using System.Threading.Tasks;
using BackEndAPI.Models;
using BackEndAPI.Interfaces;
using BackEndAPI.DBContext;

namespace BackEndAPI.Repositories
{
    public class UserRepository : AsyncGenericRepository<User>, IAsyncUserRepository
    {
        public UserRepository(AssetsManagementDBContext context) : base(context)
        {

        }
    }
}