using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEndAPI.Interfaces;
using BackEndAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, EditModel model)
        {
            try
            {
                await _userService.Update(id, model);
            }
            catch (ArgumentException e)
            {
                if (e.InnerException == null)
                {
                    return BadRequest(e.Message);
                }
                else
                {
                    return NotFound(e.Message);
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}