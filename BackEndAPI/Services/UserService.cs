using BackEndAPI.Repositories;
using BackEndAPI.Interfaces;

namespace BackEndAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IAsyncUserRepository _repository;

        public UserService(IAsyncUserRepository repository)
        {
            _repository = repository;
        }
                
    }
}