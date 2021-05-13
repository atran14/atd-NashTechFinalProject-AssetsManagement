using BackEndAPI.Repositories;

namespace BackEndAPI.Services
{
    public class UserService
    {
        private readonly UserRepository _repository;

        public UserService(UserRepository repository)
        {
            _repository = repository;
        }
                
    }
}