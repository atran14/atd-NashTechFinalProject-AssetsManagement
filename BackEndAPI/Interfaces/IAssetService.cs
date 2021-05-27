using System.Linq;
using System.Threading.Tasks;
using BackEndAPI.Entities;

namespace BackEndAPI.Interfaces
{
    public interface IAssetService
    {
        Task<IQueryable<Asset>> GetAllAssets(int userId);

        Task<IQueryable<Asset>> GetAssetsBySearching(int userId, string searchText);

        Task<Asset> GetById(int id);
    }
}