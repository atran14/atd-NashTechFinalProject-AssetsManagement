using System.Security.Claims;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BackEndAPI.Interfaces;
using BackEndAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BackEndAPI.Enums;
using BackEndAPI.Helpers;

namespace BackEndAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(AuthenticationSchemes = "Bearer", Policy = "Admin")]
        [HttpGet("{id}")]
        public async Task<UserInfo> Get(int id)
        {
            return await _userService.GetById(id);
        }

        [Authorize(AuthenticationSchemes = "Bearer", Policy = "Admin")]
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
        [HttpGet("type/{type}")]
        public async Task<ActionResult<GetUsersListPagedResponseDTO>> GetUsersByType(
            UserType type,
            [FromQuery] PaginationParameters paginationParameters
        )
        {
            var adminClaim = HttpContext.User.FindFirst(ClaimTypes.Name);
            var users = await _userService.GetUsersByType(
                paginationParameters, 
                Int32.Parse(adminClaim.Value),
                type
            );

            return Ok(users);
        }

        [HttpGet("search")]
        public async Task<ActionResult<GetUsersListPagedResponseDTO>> SearchUsers(
            [FromQuery] string query,
            [FromQuery] PaginationParameters paginationParameters
        )
        {
            var adminClaim = HttpContext.User.FindFirst(ClaimTypes.Name);
            var users = await _userService.SearchUsers(
                paginationParameters, 
                Int32.Parse(adminClaim.Value),
                query
            );

            return Ok(users);
        }
        
        [Authorize(AuthenticationSchemes = "Bearer", Policy = "Admin")]
        [HttpPut("change-password/{id}")]
        public async Task<IActionResult> ChangePassword(int id, [FromBody]ChangePasswordRequest model)
        {
            var user = _userService.GetUserByIdWithPassword(id);
            if (user == null)
            {
                return NotFound(Message.UserNotFound);
            }
            if (user.Result.Password != model.OldPassword)
            {
                return BadRequest(new {message = Message.OldPasswordIncorrect});
            }
            await _userService.ChangePassword(id, model);
            return Ok(new {message = Message.ChangePasswordSucceed});
        }
    }
}