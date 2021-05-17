using System.Collections.Generic;
using System.Threading.Tasks;
using BackEndAPI.Interfaces;
using BackEndAPI.Models;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
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
        public ActionResult<GetUsersListPagedResponseDTO> GetAllUsers(
            [FromQuery] PaginationParameters paginationParameters
        )
        {
            var users = _userService.GetUsers(paginationParameters);

            return Ok(users);
        }
    }
}