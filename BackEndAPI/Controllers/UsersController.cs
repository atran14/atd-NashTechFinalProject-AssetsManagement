using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using BackEndAPI.Interfaces;
using BackEndAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BackEndAPI.Controllers
{
    [Authorize("Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {

        private readonly IUserService _service;

        public UsersController(IUserService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<UserDTO>> GetAllUsers()
        {
            var users = _service.GetAllUsers()
                .Select(u => UserToDTO(u))
                .ToList();
            return Ok(users);
        }

        private static UserDTO UserToDTO(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("Input user is null");
            }

            return new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth,
                Gender = user.Gender,
                JoinedDate = user.JoinedDate,
                Location = user.Location,
                StaffCode = user.StaffCode,
                Type = user.Type,
            };
        }
    }
}