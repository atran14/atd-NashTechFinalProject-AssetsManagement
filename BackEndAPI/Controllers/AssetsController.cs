using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BackEndAPI.Interfaces;
using BackEndAPI.Entities;

namespace Namespace
{
    // [Authorize(AuthenticationSchemes = "Bearer", Policy = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AssetsController : ControllerBase
    {

        private readonly IAssetService _service;

        public AssetsController(IAssetService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateAssetModel model)
        {
            return Ok(await _service.Create(model));
        }

    }
}