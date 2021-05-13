using System.Linq;
using System.Threading.Tasks;
using BackEndAPI.Models;
using BackEndAPI.Interfaces;
using BackEndAPI.DBContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.RegularExpressions;

namespace BackEndAPI.Repositories
{
    public class UserRepository : AsyncGenericRepository<User>, IAsyncUserRepository
    {
        public UserRepository(AssetsManagementDBContext context) : base(context)
        { }

        public bool CheckUsernameExist(string username)
        {
            var user = _context.Set<User>().FirstOrDefaultAsync(c => c.Username == username);
            if (user != null)
            {
                return true;
            }
            return false;
        }
    }
}