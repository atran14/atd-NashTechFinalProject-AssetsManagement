using System.Linq;
using System.Threading.Tasks;
using BackEndAPI.Models;
using BackEndAPI.Interfaces;
using BackEndAPI.DBContext;
using System.Collections.Generic;

namespace BackEndAPI.Repositories
{
    public class UserRepository : AsyncGenericRepository<User>, IAsyncUserRepository
    {
        public UserRepository(AssetsManagementDBContext context) : base(context)
        {

        }
    }
}