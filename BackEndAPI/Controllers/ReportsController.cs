using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BackEndAPI.Interfaces;
using BackEndAPI.Entities;
using System.Net.Http;
using System.IO;
using BackEndAPI.Helpers;
using System.Net;

namespace BackEndAPI.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer", Policy = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet]
        public IActionResult GetReport()
        {
            var report = _reportService.GetReport();
            return Ok(report);
        }

        [HttpGet("ExportXls")]
        public async Task<HttpResponseMessage> ExportXls()
        {
            HttpRequestMessage request = new HttpRequestMessage();
            string fileName = string.Concat("Report_" + DateTime.Now.ToString("yyyy_MM_dd_hh_mm_sss") + ".xlsx");
            var folderReport = "./Report";
            string filePath = "./Report";
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            string fullPath = Path.Combine(filePath, fileName);
            try
            {
                var data = _reportService.GetReport().ToList();
                await ReportHelper.GenerateXls(data, fullPath);
                return request.CreateErrorResponse(HttpStatusCode.OK, Path.Combine(folderReport, fileName));
            }
            catch (Exception ex)
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

    }
}