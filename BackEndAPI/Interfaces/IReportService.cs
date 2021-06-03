using System.Collections.Generic;
using BackEndAPI.Models;

namespace BackEndAPI.Interfaces
{
    public interface IReportService
    {
        
        IEnumerable<ReportModel> GetReportFromHaNoi();
        IEnumerable<ReportModel> GetReportFromHoChiMinh();
        
    }
}