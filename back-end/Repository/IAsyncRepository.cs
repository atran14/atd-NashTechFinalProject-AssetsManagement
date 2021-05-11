using System.Linq;
using System.Threading.Tasks;

namespace back_end.Repository
{
    public interface IAsyncRepository<TEntity>
    {
        IQueryable<TEntity> GetAll();

        Task<TEntity> GetById(int id);

        Task Create(TEntity entity);

        Task Update(int id, TEntity entity);

        Task Delete(int id);
    }
}