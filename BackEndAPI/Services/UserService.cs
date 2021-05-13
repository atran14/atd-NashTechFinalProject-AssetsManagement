using System.Linq;
using System.Collections.Generic;
using BackEndAPI.Interfaces;
using BackEndAPI.Models;

namespace BackEndAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IAsyncUserRepository _repository;

        public UserService(IAsyncUserRepository repository)
        {
            _repository = repository;
        }

        public IQueryable<User> GetAllUsers()
        {
            return _repository.GetAll()
                .Where(u => u.Status == UserStatus.Active);
        }
    }
}