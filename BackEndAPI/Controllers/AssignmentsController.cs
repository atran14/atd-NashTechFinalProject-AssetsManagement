using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using BackEndAPI.Entities;
using BackEndAPI.Enums;
using BackEndAPI.Interfaces;
using BackEndAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
//using BackEndAPI.Models;

namespace BackEndAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentsController : ControllerBase
    {
        private readonly IAssignmentService _assignmentService;
        public AssignmentsController(IAssignmentService assignmentService)
        {
            _assignmentService = assignmentService;
        }

        [Authorize(AuthenticationSchemes = "Bearer", Policy = "Admin")]
        [HttpPost("{userId}")]
        public async Task<IActionResult> Post(int userId, AssignmentModel model)
        {
            await _assignmentService.Create(userId, model);
            return Ok();
        }
        [Authorize(AuthenticationSchemes = "Bearer", Policy = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, AssignmentModel model)
        {
            await _assignmentService.Update(id, model);
            return Ok();
        }

        [Authorize(AuthenticationSchemes = "Bearer", Policy = "Admin")]
        [HttpGet("getAllNoCondition")]
        public IQueryable<Assignment> GetAllNoCondition()
        {
            
            return  _assignmentService.GetAll();
        }

        [Authorize(AuthenticationSchemes = "Bearer", Policy = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Assignment>>> GetAllAssignments(
             [FromQuery] PaginationParameters paginationParameters
            )
        {
            var adminClaim = HttpContext.User.FindFirst(ClaimTypes.Name);
            var assignments = await _assignmentService.GetAllAssignments(paginationParameters, Int32.Parse(adminClaim.Value));

            return Ok(assignments);
        }
        [Authorize(AuthenticationSchemes = "Bearer", Policy = "Admin")]
        [HttpGet("{id}")]
        public async Task<Assignment> GetById(int id)
        {
            var assignment = await _assignmentService.GetById(id);
            return assignment;
        }

        [Authorize(AuthenticationSchemes = "Bearer", Policy = "Admin")]
        [HttpGet("state/{state}")]
        public async Task<ActionResult<GetAssignmentListPagedResponse>> GetAssignmetByType(
         AssignmentState state,
         [FromQuery] PaginationParameters paginationParameters
     )
        {
            var adminClaim = HttpContext.User.FindFirst(ClaimTypes.Name);
            var assignment = await _assignmentService.GetAssignmentByState(
                paginationParameters,
                Int32.Parse(adminClaim.Value),
                state
            );

            return Ok(assignment);
        }

        [Authorize(AuthenticationSchemes = "Bearer", Policy = "Admin")]
        [HttpGet("date/{date}")]
        public async Task<ActionResult<GetAssignmentListPagedResponse>> GetAssignmetByDate(
         DateTime date,
         [FromQuery] PaginationParameters paginationParameters
     )
        {
            var adminClaim = HttpContext.User.FindFirst(ClaimTypes.Name);
            var assignment = await _assignmentService.GetAssignmentByDate(
                paginationParameters,
                Int32.Parse(adminClaim.Value),
                date
            );

            return Ok(assignment);
        }

        [Authorize(AuthenticationSchemes = "Bearer", Policy = "Admin")]
        [HttpGet("search")]
        public async Task<ActionResult<GetAssignmentListPagedResponse>> SearchAssignments(
            [FromQuery] string query,
            [FromQuery] PaginationParameters paginationParameters
        )
        {
            var adminClaim = HttpContext.User.FindFirst(ClaimTypes.Name);
            var assignment = await _assignmentService.SearchAssignments(
                paginationParameters,
                Int32.Parse(adminClaim.Value),
                query
            );

            return Ok(assignment);
        }






    }
}