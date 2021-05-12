using System.Linq;
using System.Threading.Tasks;
using BackEndAPI.Models;
using BackEndAPI.Interfaces;

namespace BackEndAPI.Repositories
{
    public class AsyncGenericRepository<TEntity> : IAsyncRepository<TEntity>
        where TEntity : class, IEntity
    {

        public AsyncGenericRepository()
        {

        }

        public async Task Create(TEntity entity)
        {
            throw new System.NotImplementedException();
        }

        public async Task Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        public IQueryable<TEntity> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public async Task<TEntity> GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task Update(int id, TEntity entity)
        {
            throw new System.NotImplementedException();
        }
    }
}