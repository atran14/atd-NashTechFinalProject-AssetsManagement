using System.Linq;
using BackEndAPI.Repositories;
using BackEndAPI.Interfaces;
using System.Threading.Tasks;
using BackEndAPI.Models;
using BackEndAPI.Entities;
using System;

namespace BackEndAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IAsyncUserRepository _userRepository;
        private readonly IAsyncAssignmentRepository _assignmentRepository;

        public UserService(IAsyncUserRepository userRepository, IAsyncAssignmentRepository assignmentRepository)
        {
            _userRepository = userRepository;
            _assignmentRepository = assignmentRepository;
        }

        public async Task Disable(int id)
        {
            int userValid = _assignmentRepository.CountUser(id);
            var user = await _userRepository.GetById(id);
            if (user == null)
            {
                throw new InvalidOperationException("Can not find user");
            }

            if (userValid > 0)
            {
                throw new ArgumentException("User is still valid assignment");
            }
            else
            {
                user.Status = UserStatus.Disabled;
                await _userRepository.Update(user);
            }

        }

        public async Task Update(int id, EditUserModel model)
        {
            var user = await _userRepository.GetById(id);

            if (user == null)
            {
                throw new InvalidOperationException("Can not find user");
            }

            if (DateTime.Now.AddYears(-18) < model.DateOfBirth)
            {

                throw new Exception("User is under 18. Please select different date");
            }
           
            if (model.JoinedDate.DayOfWeek == DayOfWeek.Saturday
                   && model.JoinedDate.DayOfWeek == DayOfWeek.Sunday)
            {
                throw new Exception("Join Date is Saturday or Sunday. Please select different date");
            }
           
            if (model.JoinedDate < model.DateOfBirth)
            {

                throw new Exception("Join Date is not later than Date Of Birth. Please select different date");
            }
            
            user.DateOfBirth = model.DateOfBirth;
            user.JoinedDate = model.JoinedDate;
            user.Gender = model.Gender;
            user.Type = model.Type;
            await _userRepository.Update(user);
        }
    }
}