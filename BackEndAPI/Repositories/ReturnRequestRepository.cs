using BackEndAPI.DBContext;
using BackEndAPI.Entities;
using BackEndAPI.Interfaces;

namespace BackEndAPI.Repositories
{
    public class ReturnRequestRepository : AsyncGenericRepository<ReturnRequest>, IAsyncReturnRequestRepository
    {
        public ReturnRequestRepository(AssetsManagementDBContext context) : base(context)
        { }
    }
}