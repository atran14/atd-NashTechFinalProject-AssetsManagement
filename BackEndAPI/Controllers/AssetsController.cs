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

        [Authorize(AuthenticationSchemes = "Bearer", Policy = "Admin")]
        [HttpGet("getallasset/{userId}")]
        public async Task<IQueryable<Asset>> GetAll(int userId)
        {
            return await _service.GetAllAssets(userId);

        }


        [Authorize(AuthenticationSchemes = "Bearer", Policy = "Admin")]
        [HttpGet("search/{userId}/{searchText}")]
        public async Task<IQueryable<Asset>> GetUserBySearching(int userId, string searchText)
        {
            return await _service.GetAssetsBySearching(userId, searchText);

        }


        [Authorize(AuthenticationSchemes = "Bearer", Policy = "Admin")]
        [HttpGet("{id}")]
        public async Task<Asset> GetById(int id)
        {
            return await _service.GetById(id);

        }
    
    }
}