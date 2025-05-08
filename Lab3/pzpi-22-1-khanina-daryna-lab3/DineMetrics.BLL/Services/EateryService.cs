using DineMetrics.BLL.Services.Interfaces;
using DineMetrics.Core.Dto;
using DineMetrics.Core.Models;
using DineMetrics.Core.Shared;
using DineMetrics.DAL.Repositories;

namespace DineMetrics.BLL.Services;

public class EateryService : IEateryService
{
    private readonly IRepository<Eatery> _repository;

    public EateryService(IRepository<Eatery> repository)
    {
        _repository = repository;
    }
    
    public async Task<ServiceResult<EateryDto>> UpdateOperatingHours(int eateryId, string from, string to)
    {
        var eatery = await _repository.GetByIdAsync(eateryId);

        if (eatery == null)
            return ServiceResult<EateryDto>.NotFound();

        eatery.OperatingHours = from + to;

        try
        {
            await _repository.UpdateAsync(eatery);
            
            return ServiceResult<EateryDto>.Success(new EateryDto()
                { Name = eatery.Name, OperatingHours = eatery.OperatingHours });
        }
        catch (Exception ex)
        {
            return ServiceResult<EateryDto>.Failure(new Error($"An error occurred: {ex.Message}"));
        }
    }

    public async Task<ServiceResult<EateryDto>> UpdateMaximumCapacity(int eateryId, int capacity)
    {
        var eatery = await _repository.GetByIdAsync(eateryId);

        if (eatery == null)
            return ServiceResult<EateryDto>.NotFound();

        eatery.MaximumCapacity = capacity;

        try
        {
            await _repository.UpdateAsync(eatery);
            
            return ServiceResult<EateryDto>.Success(new EateryDto()
                { Name = eatery.Name, MaximumCapacity = eatery.MaximumCapacity });
        }
        catch (Exception ex)
        {
            return ServiceResult<EateryDto>.Failure(new Error($"An error occurred: {ex.Message}"));
        }
    }

    public async Task<ServiceResult<EateryDto>> UpdateTemperatureThreshold(int eateryId, double minTemperature)
    {
        var eatery = await _repository.GetByIdAsync(eateryId);

        if (eatery == null)
            return ServiceResult<EateryDto>.NotFound();

        eatery.TemperatureThreshold = minTemperature;

        try
        {
            await _repository.UpdateAsync(eatery);
            
            return ServiceResult<EateryDto>.Success(new EateryDto()
                { Name = eatery.Name, TemperatureThreshold = eatery.TemperatureThreshold });
        }
        catch (Exception ex)
        {
            return ServiceResult<EateryDto>.Failure(new Error($"An error occurred: {ex.Message}"));
        }
    }
}