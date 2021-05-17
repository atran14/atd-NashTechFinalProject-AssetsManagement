using System.Linq;
using System.Threading.Tasks;
using BackEndAPI.Entities;
using BackEndAPI.Interfaces;
using BackEndAPI.DBContext;
using System;

namespace BackEndAPI.Repositories
{
    public class UserRepository : AsyncGenericRepository<User>, IAsyncUserRepository
    {
        public UserRepository(AssetsManagementDBContext context) : base(context)
        { }

        public int CountUsername(string username)
        {
            if (username == null)
            {

                throw new ArgumentNullException("Username can not be null!");

            }

            return _context.Set<User>().Where(c => c.UserName.Contains(username)).Count();

        }
    }
}