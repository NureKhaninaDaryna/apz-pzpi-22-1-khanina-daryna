using DineMetrics.Core.Dto;
using DineMetrics.Core.Shared;

namespace DineMetrics.BLL.Services.Interfaces;

public interface IEateryService
{
    Task<ServiceResult<EateryDto>> UpdateOperatingHours(int eateryId, string from, string to);
    Task<ServiceResult<EateryDto>> UpdateMaximumCapacity(int eateryId, int capacity);
    Task<ServiceResult<EateryDto>> UpdateTemperatureThreshold(int eateryId, double minTemperature);
}