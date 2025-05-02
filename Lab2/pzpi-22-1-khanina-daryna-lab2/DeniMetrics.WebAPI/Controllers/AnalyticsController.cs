using DeniMetrics.WebAPI.Attributes;
using DineMetrics.BLL.Services.Interfaces;
using DineMetrics.Core.Dto;
using DineMetrics.Core.Enums;
using Microsoft.AspNetCore.Mvc;

namespace DeniMetrics.WebAPI.Controllers;

[Authorize]
public class AnalyticsController : BaseController
{
    private readonly IAnalyticsService _analyticsService;

    public AnalyticsController(IAnalyticsService analyticsService)
    {
        _analyticsService = analyticsService;
    }
    
    [HttpGet("dashboard-metrics")]
    [PermissionAuthorize(ManagementName.AnalyticsManagement, PermissionAccess.Read)]
    public async Task<ActionResult<DashboardDataDto>> GetDashboardMetrics([FromQuery] DateTime from, [FromQuery] DateTime to)
    {
        var result = await _analyticsService.GetDashboardMetrics(from, to);

        if (!result.IsSuccess)
        {
            if (result.Error?.Message == "Not found")
                return NotFound(new { Message = "No metrics found for the specified period." });

            return BadRequest(new { Message = result.Error?.Message });
        }

        return Ok(result.Value);
    }
    
    [HttpGet("trends/{facilityId:int}")]
    [PermissionAuthorize(ManagementName.AnalyticsManagement, PermissionAccess.Read)]
    public async Task<IActionResult> GenerateTrends(int facilityId)
    {
        var result = await _analyticsService.GenerateTrends(facilityId);

        if (!result.IsSuccess)
        {
            if (result.Error?.Message == "Not found")
                return NotFound(new { Message = "No metrics found for the specified facility." });

            return BadRequest(new { Message = result.Error?.Message });
        }

        return Ok(result.Value);
    }
    
    [HttpGet("recommendations")]
    public async Task<ActionResult<PeakLoadRecommendationDto>> GetRecommendations(int eateryId, DateTime from, DateTime to)
    {
        var result = await _analyticsService.GetPeakLoadRecommendations(eateryId, from, to);
        return Ok(result.Value);
    }

    [HttpGet("anomalies")]
    public async Task<ActionResult<AnomalyDetectionDto>> GetAnomalies(int eateryId, DateTime from, DateTime to)
    {
        var result = await _analyticsService.DetectAnomalies(eateryId, from, to);
        return Ok(result.Value);
    }

    [HttpGet("forecast")]
    public async Task<ActionResult<LoadForecastDto>> GetForecast(int eateryId, int daysAhead)
    {
        var result = await _analyticsService.ForecastLoad(eateryId, daysAhead);
        return Ok(result.Value);
    }
}