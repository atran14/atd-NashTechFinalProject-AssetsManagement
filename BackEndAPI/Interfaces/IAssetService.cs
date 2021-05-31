using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEndAPI.Entities;
using BackEndAPI.Enums;
using BackEndAPI.Models;

namespace BackEndAPI.Interfaces
{
    public interface IAssetService
    {

        Task<Asset> Create(CreateAssetModel model);

        Task<IQueryable<Asset>> GetAllAssets(int userId);

        Task<IQueryable<Asset>> GetAssetsBySearching(int userId, string searchText);

        Task<Asset> GetById(int id);
    }
}