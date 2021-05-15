using System.Linq;
using BackEndAPI.Models;

namespace BackEndAPI.Interfaces
{
    public interface IAsyncUserRepository : IAsyncRepository<User>
    { }
}