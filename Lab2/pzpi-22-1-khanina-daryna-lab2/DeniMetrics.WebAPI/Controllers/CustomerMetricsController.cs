using DeniMetrics.WebAPI.Attributes;
using DineMetrics.BLL.Services.Interfaces;
using DineMetrics.Core.Dto;
using DineMetrics.Core.Models;
using DineMetrics.DAL.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DeniMetrics.WebAPI.Controllers;

public class CustomerMetricsController : BaseController
{
    private readonly IRepository<CustomerMetric> _customerMetricRepository;
    private readonly IRepository<Device> _deviceRepository;
    private readonly IMetricService _metricService;

    public CustomerMetricsController(
        IRepository<CustomerMetric> customerMetricRepository, 
        IRepository<Device> deviceRepository,
        IRepository<Report> reportRepository,
        IMetricService metricService)
    {
        _customerMetricRepository = customerMetricRepository;
        _deviceRepository = deviceRepository;
        _metricService = metricService;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<List<CustomerMetricDto>>> GetAll()
    {
        var customerMetrics = await _customerMetricRepository.GetAllAsync();
        
        var customerMetricDtos = customerMetrics.Select(metric => new CustomerMetricDto
        {
            Count = metric.Count,
            Time = metric.Time,
            DeviceId = metric.Device.Id
        }).ToList();

        return customerMetricDtos;
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<CustomerMetricDto>> GetById(int id)
    {
        var result = await _customerMetricRepository.GetByIdAsync(id);

        if (result is null)
            return BadRequest("CustomerMetric not found");
        
        return new CustomerMetricDto()
        {
            Count = result.Count,
            Time = result.Time,
            DeviceId = result.Device.Id
        };
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CustomerMetricDto dto)
    {
        var device = await _deviceRepository.GetByIdAsync(dto.DeviceId);

        if (device is null)
            return BadRequest("Device not found");
        
        var metric = new CustomerMetric
        {
            Count = dto.Count,
            Time = dto.Time,
            Device = device
        };
        
        var result = await _metricService.CreateCustomerMetric(metric);
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
        await _customerMetricRepository.RemoveByIdAsync(id);
        
        return Ok();
    }
}
