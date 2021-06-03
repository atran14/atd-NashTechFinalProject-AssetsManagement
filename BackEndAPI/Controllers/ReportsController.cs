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

        [HttpGet("report/HaNoi")]
        public IActionResult GetReportFromHaNoi()
        {
            var report = _reportService.GetReportFromHaNoi();
            return Ok(report);
        }
        [HttpGet("report/HoChiMinh")]
        public IActionResult GetReportFromHoChiMinh()
        {
            var report = _reportService.GetReportFromHoChiMinh();
            return Ok(report);
        }

        [HttpGet("exportXls/HaNoi")]
        public async Task<HttpResponseMessage> ExportXlsFromHaNoi()
        {
            HttpRequestMessage request = new HttpRequestMessage();
            string fileName = string.Concat("Report_HaNoi_" + DateTime.Now.ToString("yyyy_MM_dd") + ".xlsx");
            var folderReport = "./Reports";
            string filePath = "C:/" + folderReport;
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            string fullPath = Path.Combine(filePath, fileName);
            try
            {
                var data = _reportService.GetReportFromHaNoi().ToList();
                await ReportHelper.GenerateXls(data, fullPath);
                return request.CreateErrorResponse(HttpStatusCode.OK, Path.Combine(folderReport, fileName));
            }
            catch (Exception ex)
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
        [HttpGet("exportXls/HoChiMinh")]
        public async Task<HttpResponseMessage> ExportXlsFromHoChiMinh()
        {
            HttpRequestMessage request = new HttpRequestMessage();
            string fileName = string.Concat("Report_HoChiMinh_" + DateTime.Now.ToString("yyyy_MM_dd") + ".xlsx");
            var folderReport = "./Reports";
            string filePath = "C:/" + folderReport;
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            string fullPath = Path.Combine(filePath, fileName);
            try
            {
                var data = _reportService.GetReportFromHoChiMinh().ToList();
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