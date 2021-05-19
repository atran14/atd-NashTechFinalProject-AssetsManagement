
using BackEndAPI.Entities;

namespace BackEndAPI.Interfaces
{
    public interface IAsyncAssignmentRepository : IAsyncRepository<Assignment>
    {
        int GetCountUser(int id);
    }
}