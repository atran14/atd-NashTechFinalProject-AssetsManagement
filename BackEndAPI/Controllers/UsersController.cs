using System.Security.Claims;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BackEndAPI.Interfaces;
using BackEndAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BackEndAPI.Helpers;

namespace BackEndAPI.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer", Policy = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<UserInfo> Get(int id)
        {
            return await _userService.GetById(id);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateUserModel user)
        {
            return Ok(await _userService.Create(user));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, EditUserModel model)
        {
            await _userService.Update(id, model);
            return Ok();
        }

        [HttpPut("disable/{id}")]
        public async Task<IActionResult> Disabled(int id)
        {
            await _userService.Disable(id);
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<GetUsersListPagedResponseDTO>> GetAllUsers(
            [FromQuery] PaginationParameters paginationParameters
        )
        {
            var adminClaim = HttpContext.User.FindFirst(ClaimTypes.Name);
            var users = await _userService.GetUsers(paginationParameters, Int32.Parse(adminClaim.Value));

            return Ok(users);
        }

        //Change Password
        [HttpPut("change-password/{id}")]
        public async Task<IActionResult> ChangePassword(int id, string oldPassword, string newPassword)
        {
            await _userService.ChangePassword(id, oldPassword, newPassword);
            return Ok(new {message = Message.ChangePasswordSucceed});
        }
    }
}