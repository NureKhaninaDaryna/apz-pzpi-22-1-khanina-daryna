using DineMetrics.Core.Models;
using DineMetrics.Core.Shared;

namespace DineMetrics.BLL.Services.Interfaces;

public interface IMetricService
{
    Task<ServiceResult> CreateTemperatureMetric(TemperatureMetric metric);
    Task<ServiceResult> CreateCustomerMetric(CustomerMetric metric);
}