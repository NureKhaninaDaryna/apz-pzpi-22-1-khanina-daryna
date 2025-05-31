using System.Globalization;
using DeniMetrics.WebAPI.Attributes;
using DineMetrics.BLL.Helpers;
using DineMetrics.Core.Dto;
using DineMetrics.Core.Enums;
using DineMetrics.Core.Models;
using DineMetrics.DAL.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DeniMetrics.WebAPI.Controllers;

[Authorize]
public class ReportsController : BaseController
{
    private readonly IRepository<Report> _reportRepository;

    public ReportsController(IRepository<Report> reportRepository)
    {
        _reportRepository = reportRepository;
    }

    [HttpGet]
    [PermissionAuthorize(ManagementName.AnalyticsManagement, PermissionAccess.Read)]
    public async Task<ActionResult<List<ReportDto>>> GetAll()
    {
        var reports = await _reportRepository.GetAllAsync();
        
        var reportDtos = reports.Select(report => new ReportDto
        {
            ReportDate = report.ReportDate,
            TotalCustomers = report.TotalCustomers,
            AverageTemperature = report.AverageTemperature
        }).ToList();
        
        return reportDtos;
    }

    [HttpGet("{id}")]
    [PermissionAuthorize(ManagementName.AnalyticsManagement, PermissionAccess.Read)]
    public async Task<ActionResult<ReportDto>> GetById(int id)
    {
        var result = await _reportRepository.GetByIdAsync(id);

        if (result is null)
            return BadRequest("Report not found");
        
        return new ReportDto
        {
            ReportDate = result.ReportDate,
            TotalCustomers = result.TotalCustomers,
            AverageTemperature = result.AverageTemperature
        };
    }
    
    [HttpPost("download")]
    [PermissionAuthorize(ManagementName.AnalyticsManagement, PermissionAccess.Full)]
    public async Task<IActionResult> GetReportsByDateRange(ReportDownloadDto model)
    {
        if (model.EndDate < model.StartDate)
        {
            return BadRequest("End date cannot be before start date.");
        }
        
        var reports = await _reportRepository
            .GetByPredicateAsync(r => r.ReportDate >= model.StartDate && r.ReportDate <= model.EndDate);
        
        if (reports.Count == 0)
        {
            return NotFound("No reports found for the given date range.");
        }
        
        var data = reports.Select(r => new List<string>
        {
            r.ReportDate.ToString(),
            Math.Round(r.AverageTemperature, 2).ToString(CultureInfo.InvariantCulture),
            r.TotalCustomers.ToString()
        }).ToList();
        
        var headers = new List<string> { "Date", "Average Temperature", "Total Customers" };
        
        var pdfBytes = PdfGeneratorService.CreatePdf(data, "Reports from " + model.StartDate + " to " + model.EndDate, headers);
        
        return File(pdfBytes, "application/pdf", "Reports_" + model.StartDate + "_to_" + model.EndDate + ".pdf");
    }
}

public class ReportDownloadDto
{
    public DateOnly StartDate { get; set; }
    
    public DateOnly EndDate { get; set; }
}
