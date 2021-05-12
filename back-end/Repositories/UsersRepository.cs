using System.Linq;
using System.Threading.Tasks;
using BackEndAPI.Models;
using BackEndAPI.Repository;

namespace BackEndAPI.Services
{
    public class UsersRepository : IAsyncRepository<User>
    {
        public async Task Create(User entity)
        {
            throw new System.NotImplementedException();
        }

        public async Task Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        public IQueryable<User> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public async Task<User> GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task Update(int id, User entity)
        {
            throw new System.NotImplementedException();
        }
    }
}