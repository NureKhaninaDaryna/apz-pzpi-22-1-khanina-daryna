using DineMetrics.BLL.Services.Interfaces;
using DineMetrics.Core.Dto;
using DineMetrics.Core.Models;
using DineMetrics.Core.Shared;
using DineMetrics.DAL.Repositories;

namespace DineMetrics.BLL.Services;

public class AnalyticsService(
    IRepository<TemperatureMetric> temperatureMetricRepository,
    IRepository<CustomerMetric> customerMetricRepository)
    : IAnalyticsService
{
    public async Task<ServiceResult<DashboardDataDto>> GetDashboardMetrics(DateTime from, DateTime to)
    {
        try
        {
            var metrics = await temperatureMetricRepository.GetByPredicateAsync(m => m.Time >= from && m.Time <= to);

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
            var metrics = await temperatureMetricRepository.GetByPredicateAsync(m => m.Device.Eatery.Id == facilityId);

            if (metrics.Count == 0)
                return ServiceResult<List<TrendAnalysisDto>>.NotFound();

            var trends = metrics
                .GroupBy(m => m.Time.Date)
                .Select(g => new TrendAnalysisDto
                {
                    Date = g.Key,
                    AverageValue = Math.Truncate(g.Average(m => m.Value) * 10) / 10
                }).ToList();

            return ServiceResult<List<TrendAnalysisDto>>.Success(trends);
        }
        catch (Exception ex)
        {
            return ServiceResult<List<TrendAnalysisDto>>.Failure(new Error($"An error occurred: {ex.Message}"));
        }
    }
    
    public async Task<ServiceResult<PeakLoadRecommendationDto>> GetPeakLoadRecommendations(int eateryId, DateTime from, DateTime to)
    {
        var metrics = await customerMetricRepository.GetByPredicateAsync(m =>
            m.Device.Eatery.Id == eateryId && m.Time >= from && m.Time <= to);

        var grouped = metrics
            .GroupBy(m => m.Time.Hour)
            .Select(g => new { Hour = g.Key, AverageVisitors = g.Average(m => m.Count) })
            .OrderByDescending(x => x.AverageVisitors)
            .ToList();

        var peakHours = grouped.Take(3).Select(x => x.Hour).ToList();

        return ServiceResult<PeakLoadRecommendationDto>.Success(new PeakLoadRecommendationDto
        {
            RecommendedStaffIncreaseHours = peakHours
        });
    }

    public async Task<ServiceResult<AnomalyDetectionDto>> DetectAnomalies(int eateryId, DateTime from, DateTime to)
    {
        var metrics = await temperatureMetricRepository.GetByPredicateAsync(m =>
            m.Device.Eatery.Id == eateryId && m.Time >= from && m.Time <= to);

        var average = metrics.Average(m => m.Value);
        var stdDev = Math.Sqrt(metrics.Average(m => Math.Pow(m.Value - average, 2)));

        var anomalies = metrics
            .Where(m => Math.Abs(m.Value - average) > 2 * stdDev)
            .ToList();

        return ServiceResult<AnomalyDetectionDto>.Success(new AnomalyDetectionDto
        {
            AnomalyTimestamps = anomalies.Select(a => a.Time).ToList(),
            Threshold = average,
            StandardDeviation = stdDev
        });
    }

    public async Task<ServiceResult<LoadForecastDto>> ForecastLoad(int eateryId, int daysAhead)
    {
        var metrics = await customerMetricRepository.GetByPredicateAsync(m => m.Device.Eatery.Id == eateryId);
        var dailyGroups = metrics.GroupBy(m => m.Time.Date)
            .Select(g => new { Date = g.Key, Count = g.Sum(m => m.Count) })
            .OrderBy(g => g.Date)
            .ToList();

        var averageLoad = dailyGroups.Average(x => x.Count);

        var forecast = new List<ForecastDay>();
        for (var i = 1; i <= daysAhead; i++)
        {
            forecast.Add(new ForecastDay
            {
                Date = DateTime.Today.AddDays(i),
                ExpectedVisitors = (int)(averageLoad + (i % 3 == 0 ? 20 : 0))
            });
        }

        return ServiceResult<LoadForecastDto>.Success(new LoadForecastDto
        {
            Forecast = forecast
        });
    }
}