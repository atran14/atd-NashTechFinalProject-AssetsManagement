using System.Threading.Tasks;
using BackEndAPI.Models;

namespace BackEndAPI.Interfaces
{
    public interface IAsyncAssignmentRepository : IAsyncRepository<Assignment>
    {
        int CountUser(int id);
    }
}