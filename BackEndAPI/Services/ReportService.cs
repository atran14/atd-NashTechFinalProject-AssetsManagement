using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BackEndAPI.Enums;
using BackEndAPI.Interfaces;
using BackEndAPI.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;

namespace BackEndAPI.Services
{
    public class ReportService : IReportService
    {
        private readonly IAsyncAssetRepository _assetRepository;
        private readonly IAsyncAssetCategoryRepository _categoryRepository;
        public IConfiguration _configuration { get; }
        public ReportService(IAsyncAssetRepository assetRepository, IAsyncAssetCategoryRepository categoryRepository, IConfiguration configuration)
        {
            _assetRepository = assetRepository;
            _categoryRepository = categoryRepository;
            _configuration = configuration;
        }

    public IEnumerable<ReportModel> GetReport()
    {
        IList<ReportModel> reportList = new List<ReportModel>();
        try{
            var con= _configuration.GetConnectionString("SqlConnection");
            using (SqlConnection connection = new SqlConnection(con))
                {
                    connection.Open();       

                    string sql = "SELECT CategoryName"
                                +        ",Total = (SELECT COUNT(A.Id) " 
                                +                    "FROM [Assets] A "
                                +                    "WHERE A.CategoryId = AC.Id)"
                                +        ",Assigned = (SELECT COUNT(A.Id)" 
                                +                    "FROM [Assets] A "
                                +                    "WHERE A.CategoryId = AC.Id AND A.State = 2)"
                                +        ",Available = (SELECT COUNT(A.Id) "
                                +                    "FROM [Assets] A "
                                +                    "WHERE A.CategoryId = AC.Id AND A.State = 0)"
                                +        ",NotAvailable = (SELECT COUNT(A.Id) "
                                +                    "FROM [Assets] A "
                                +                    "WHERE A.CategoryId = AC.Id AND A.State = 1)"
                                +        ",WaitingForRecycling = (SELECT COUNT(A.Id) "
                                +                    "FROM [Assets] A "
                                +                    "WHERE A.CategoryId = AC.Id AND A.State = 3)"
                                +        ",Recycled = (SELECT COUNT(A.Id) "
                                +                    "FROM [Assets] A "
                                +                    "WHERE A.CategoryId = AC.Id AND A.State = 4)"
                                +"FROM AssetCategories AC"
                                +"ORDER BY AC.CategoryCode";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            int count = 1;
                            while (reader.Read())
                            {
                                // Console.WriteLine("{0} {1}", reader.GetString(0), reader.GetString(1));
                                reportList.Add(new ReportModel
                                {
                                    ID = count,
                                    CategoryName = reader["CategoryName"].ToString(),
                                    Total = Int32.Parse(reader["Total"].ToString()),
                                    Assigned = Int32.Parse(reader["Assigned"].ToString()),
                                    Available = Int32.Parse(reader["Available"].ToString()),
                                    NotAvailable = Int32.Parse(reader["NotAvailable"].ToString()),
                                    WaitingForRecycling = Int32.Parse(reader["WaitingForRecycling"].ToString()),
                                    Recycled = Int32.Parse(reader["Recycled"].ToString()),
                                });
                                count++;
                            }
                        }
                    }                    
                }
        }catch (SqlException ex)
        {
            throw ex;
        }

        return reportList;
    }
  }
}