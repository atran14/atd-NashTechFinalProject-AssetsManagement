using System.Linq;
using BackEndAPI.Entities;
using BackEndAPI.Interfaces;
using BackEndAPI.DBContext;

namespace BackEndAPI.Repositories
{
    public class UserRepository : AsyncGenericRepository<User>, IAsyncUserRepository
    {
        public UserRepository(AssetsManagementDBContext context) : base(context)
        { }

        public int CountUsername(string username)
        {
            
            return _context.Set<User>().Where(c => c.UserName.Contains(username)).Count();

        }
    }
}