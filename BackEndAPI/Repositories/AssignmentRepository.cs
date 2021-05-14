using System.Linq;
using System.Threading.Tasks;
using BackEndAPI.Entities;
using BackEndAPI.Interfaces;
using BackEndAPI.DBContext;

namespace BackEndAPI.Repositories
{
    public class AssignmentRepository : AsyncGenericRepository<Assignment>, IAsyncAssignmentRepository
    {
        public AssignmentRepository(AssetsManagementDBContext context) : base(context)
        {

        }

        public int CountUser(int id)
        {
            return _context.Assignments.Count(x => x.AssignedToUserId == id 
            && x.State != AssignmentState.Declined);
        }
    }
}