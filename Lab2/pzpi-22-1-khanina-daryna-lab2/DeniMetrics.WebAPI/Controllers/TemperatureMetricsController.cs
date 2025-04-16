using DeniMetrics.WebAPI.Attributes;
using DineMetrics.BLL.Services.Interfaces;
using DineMetrics.Core.Dto;
using DineMetrics.Core.Models;
using DineMetrics.DAL.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DeniMetrics.WebAPI.Controllers;

public class TemperatureMetricsController : BaseController
{
    private readonly IRepository<TemperatureMetric> _temperatureMetricRepository;
    private readonly IRepository<Device> _deviceRepository;
    private readonly IMetricService _metricService;

    public TemperatureMetricsController(
        IRepository<TemperatureMetric> temperatureMetricRepository, 
        IRepository<Device> deviceRepository,
        IMetricService metricService)
    {
        _temperatureMetricRepository = temperatureMetricRepository;
        _deviceRepository = deviceRepository;
        _metricService = metricService;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<List<TemperatureMetricDto>>> GetAll()
    {
        var temperatureMetrics = await _temperatureMetricRepository.GetAllAsync();
        
        var temperatureMetricDtos = temperatureMetrics.Select(metric => new TemperatureMetricDto
        {
            DeviceId = metric.Device.Id,
            Value = metric.Value,
            Time = metric.Time,
        }).ToList();
        
        return temperatureMetricDtos;
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<TemperatureMetricDto>> GetById(int id)
    {
        var result = await _temperatureMetricRepository.GetByIdAsync(id);

        if (result is null)
            return BadRequest("TemperatureMetric not found");
        
        return new TemperatureMetricDto
        {
            DeviceId = result.Device.Id,
            Value = result.Value,
            Time = result.Time,
        };
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] TemperatureMetricDto dto)
    {
        var device = await _deviceRepository.GetByIdAsync(dto.DeviceId);

        if (device is null)
            return BadRequest("Device not found");
        
        var metric = new TemperatureMetric
        {
            Value = dto.Value,
            Time = dto.Time,
            Device = device
        };
        
        var result = await _metricService.CreateTemperatureMetric(metric);
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return CreatedAtAction(nameof(GetById), new { id = metric.Id }, dto);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<ActionResult> Delete(int id)
    {
        await _temperatureMetricRepository.RemoveByIdAsync(id);
        
        return Ok();
    }
}