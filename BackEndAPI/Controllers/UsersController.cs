using System.Linq;
using System.Collections.Generic;
using BackEndAPI.Interfaces;
using BackEndAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BackEndAPI.Controllers
{
    // [Authorize("Admin")]
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
        public ActionResult<GetUsersListPagedResponseDTO> GetAllUsers(
            [FromQuery] PaginationParameters paginationParameters
        )
        {
            var users = _service.GetUsers(paginationParameters);

            return Ok(users);
        }

    }
}