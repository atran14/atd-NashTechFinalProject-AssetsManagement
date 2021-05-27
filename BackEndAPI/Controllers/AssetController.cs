using System.Security.Claims;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BackEndAPI.Interfaces;
using BackEndAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using BackEndAPI.Entities;

namespace BackEndAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetController : ControllerBase
    {
        private readonly IAssetService _assetService;
        public AssetController(IAssetService assetService)
        {
            _assetService = assetService;
        }

        [Authorize(AuthenticationSchemes = "Bearer", Policy = "Admin")]
        [HttpGet("getallasset/{userId}")]
        public async Task<IQueryable<Asset>> GetAll(int userId)
        {
            return await _assetService.GetAllAssets(userId);

        }


        [Authorize(AuthenticationSchemes = "Bearer", Policy = "Admin")]
        [HttpGet("search/{userId}/{searchText}")]
        public async Task<IQueryable<Asset>> GetUserBySearching(int userId, string searchText)
        {
            return await _assetService.GetAssetsBySearching(userId, searchText);

        }


        [Authorize(AuthenticationSchemes = "Bearer", Policy = "Admin")]
        [HttpGet("{id}")]
        public async Task<Asset> GetById(int id)
        {
            return await _assetService.GetById(id);

        }


    }
}