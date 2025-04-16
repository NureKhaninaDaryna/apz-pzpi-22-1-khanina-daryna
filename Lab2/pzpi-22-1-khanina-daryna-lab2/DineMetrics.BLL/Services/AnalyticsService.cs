using DineMetrics.BLL.Services.Interfaces;
using DineMetrics.Core.Dto;
using DineMetrics.Core.Models;
using DineMetrics.Core.Shared;
using DineMetrics.DAL.Repositories;

namespace DineMetrics.BLL.Services;

public class AnalyticsService : IAnalyticsService
{
    private readonly IRepository<TemperatureMetric> _metricRepository;

    public AnalyticsService(IRepository<TemperatureMetric> metricRepository)
    {
        _metricRepository = metricRepository;
    }

    public async Task<ServiceResult<DashboardDataDto>> GetDashboardMetrics(DateTime from, DateTime to)
    {
        try
        {
            var metrics = await _metricRepository.GetByPredicateAsync(m => m.Time >= from && m.Time <= to);

            if (metrics.Count == 0)
                return ServiceResult<DashboardDataDto>.NotFound();

            var avgTemp = metrics.Average(m => m.Value);

            var dashboardData = new DashboardDataDto
            {
                AverageTemperature = avgTemp,
                TotalMetrics = metrics.Count
            };

            return ServiceResult<DashboardDataDto>.Success(dashboardData);
        }
        catch (Exception ex)
        {
            return ServiceResult<DashboardDataDto>.Failure(new Error($"An error occurred: {ex.Message}"));
        }
    }

    public async Task<ServiceResult<List<TrendAnalysisDto>>> GenerateTrends(int facilityId)
    {
        try
        {
            var metrics = await _metricRepository.GetByPredicateAsync(m => m.Device.Eatery.Id == facilityId);

            if (metrics.Count == 0)
                return ServiceResult<List<TrendAnalysisDto>>.NotFound();

            var trends = metrics
                .GroupBy(m => m.Time.Date)
                .Select(g => new TrendAnalysisDto
                {
                    Date = g.Key,
                    AverageValue = g.Average(m => m.Value)
                }).ToList();

            return ServiceResult<List<TrendAnalysisDto>>.Success(trends);
        }
        catch (Exception ex)
        {
            return ServiceResult<List<TrendAnalysisDto>>.Failure(new Error($"An error occurred: {ex.Message}"));
        }
    }
}