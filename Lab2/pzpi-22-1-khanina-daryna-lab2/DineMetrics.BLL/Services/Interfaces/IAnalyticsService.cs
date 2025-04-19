using DineMetrics.Core.Dto;
using DineMetrics.Core.Shared;

namespace DineMetrics.BLL.Services.Interfaces;

public interface IAnalyticsService
{
    Task<ServiceResult<DashboardDataDto>> GetDashboardMetrics(DateTime from, DateTime to);
    Task<ServiceResult<List<TrendAnalysisDto>>> GenerateTrends(int facilityId);
    Task<ServiceResult<PeakLoadRecommendationDto>> GetPeakLoadRecommendations(int eateryId, DateTime from, DateTime to);
    Task<ServiceResult<AnomalyDetectionDto>> DetectAnomalies(int eateryId, DateTime from, DateTime to);
    Task<ServiceResult<LoadForecastDto>> ForecastLoad(int eateryId, int daysAhead);
}