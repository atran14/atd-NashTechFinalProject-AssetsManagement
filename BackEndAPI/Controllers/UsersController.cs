using System.Security.Claims;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BackEndAPI.Interfaces;
using BackEndAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BackEndAPI.Enums;

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

        [HttpGet("type/{type}")]
        public async Task<ActionResult<GetUsersListPagedResponseDTO>> GetUsersByType(
            UserType type,
            [FromQuery] PaginationParameters paginationParameters
        )
        {
            var adminClaim = HttpContext.User.FindFirst(ClaimTypes.Name);
            var users = await _userService.GetUsers(
                paginationParameters, 
                Int32.Parse(adminClaim.Value),
                type
            );

            return Ok(users);
        }

        [HttpGet("fullName/{searchText}")]
        public async Task<ActionResult<GetUsersListPagedResponseDTO>> SearchUserByFullName(
        // public void SearchUserByFullName(
            string searchText,
            [FromQuery] PaginationParameters paginationParameters
        )
        {
            var adminClaim = HttpContext.User.FindFirst(ClaimTypes.Name);
            var users = await _userService.SearchUsersByFullName(
                paginationParameters, 
                Int32.Parse(adminClaim.Value),
                searchText
            );

            return Ok(users);
        }

        [HttpGet("staffCode/{searchText}")]
        public async Task<ActionResult<GetUsersListPagedResponseDTO>> SearchUserByStaffCode(
        // public void SearchUserByStaffCode(
            string searchText,
            [FromQuery] PaginationParameters paginationParameters
        )
        {
            var adminClaim = HttpContext.User.FindFirst(ClaimTypes.Name);
            var users = await _userService.SearchUsersByStaffCode(
                paginationParameters, 
                Int32.Parse(adminClaim.Value),
                searchText
            );

            return Ok(users);
        }
    }
}