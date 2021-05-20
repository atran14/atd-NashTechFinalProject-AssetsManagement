using System.Linq;
using BackEndAPI.Entities;
using BackEndAPI.Interfaces;
using BackEndAPI.DBContext;
using System.Collections.Generic;
using System;
using BackEndAPI.Helpers;

namespace BackEndAPI.Repositories
{
    public class UserRepository : AsyncGenericRepository<User>, IAsyncUserRepository
    {
        public UserRepository(AssetsManagementDBContext context) : base(context)
        { }

        public int CountUsername(string username)
        {
            if (username == null || username == "")
            {

                throw new ArgumentNullException(Message.NullOrEmptyUsername);

            }

            return _context.Set<User>().Where(c => c.UserName.Contains(username)).Count();

        }
    }
}