using System.Linq;
using BackEndAPI.Entities;
using BackEndAPI.Interfaces;
using BackEndAPI.DBContext;
using BackEndAPI.Enums;

namespace BackEndAPI.Repositories
{
    public class AssignmentRepository : AsyncGenericRepository<Assignment>, IAsyncAssignmentRepository
    {
        public AssignmentRepository(AssetsManagementDBContext context) : base(context)
        {

        }

        public int GetCountUser(int id)
        {
            return _context.Assignments.Count(x => x.AssignedToUserId == id 
            && x.State != AssignmentState.Declined);
        }   
    }
}