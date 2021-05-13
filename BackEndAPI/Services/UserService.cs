using BackEndAPI.Repositories;
using BackEndAPI.Interfaces;
using System.Threading.Tasks;
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

        public async Task Update(int id, EditModel model)
        {
            var user = await _repository.GetById(id);
            try
            {
                if (user != null)
                {
                    user.DateOfBirth = model.DateOfBirth;
                    user.JoinedDate = model.JoinedDate;
                    user.Gender = model.Gender;
                    user.Type = model.Type;
                    await _repository.Update(user);
                }
            }
            catch
            {

            }
        }
    }
}