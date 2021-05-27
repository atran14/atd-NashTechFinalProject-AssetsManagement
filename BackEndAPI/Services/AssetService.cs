using System;
using System.Linq;
using System.Threading.Tasks;
using BackEndAPI.Entities;
using BackEndAPI.Enums;
using BackEndAPI.Interfaces;

namespace BackEndAPI.Services
{
    public class AssetService : IAssetService
    {
        private readonly IAsyncAssetRepository _repository;
        private readonly IAsyncUserRepository _userrepository;
        public AssetService(IAsyncAssetRepository repository, IAsyncUserRepository userrepository)
        {
            _repository = repository;
            _userrepository = userrepository;
        }
        public async Task<IQueryable<Asset>> GetAllAssets(int userId)
        {
            var user = await _userrepository.GetById(userId);
            if (user == null)
            {
                throw new InvalidOperationException("Can not find user");
            }
            var listAsset = _repository.GetAll()
                               .Where(x => x.Location == user.Location && x.State == AssetState.Available)
                               .AsQueryable();

            return listAsset;
        }

        public async Task<IQueryable<Asset>> GetAssetsBySearching(int userId, string searchText)
        {
            var user = await _userrepository.GetById(userId);
            if (user == null)
            {
                throw new InvalidOperationException("Can not find user");
            }
            var listAsset = _repository.GetAll()
                               .Where(x => x.Location == user.Location 
                               && x.State == AssetState.Available
                               && (x.AssetCode.Contains(searchText)
                               || x.AssetName.Contains(searchText)))
                               .AsQueryable();
            return listAsset;
        }

        public async Task<Asset> GetById(int id)
        {
            var asset = await _repository.GetById(id);
            if(asset == null)
            {
                throw new InvalidOperationException("Can not find asset");
            }

            return asset;
        }
    }
}