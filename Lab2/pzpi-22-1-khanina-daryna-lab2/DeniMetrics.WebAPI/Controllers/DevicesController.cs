using DeniMetrics.WebAPI.Attributes;
using DineMetrics.Core.Dto;
using DineMetrics.Core.Models;
using DineMetrics.DAL.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DeniMetrics.WebAPI.Controllers;

public class DevicesController : BaseController
{
    private readonly IRepository<Device> _deviceRepository;
    private readonly IRepository<Eatery> _eateryRepository;

    public DevicesController(IRepository<Device> deviceRepository, IRepository<Eatery> eateryRepository)
    {
        _deviceRepository = deviceRepository;
        _eateryRepository = eateryRepository;
    }
    
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<List<DeviceDto>>> GetAll()
    {
        var devices = await _deviceRepository.GetAllAsync();
        
        var devicesDtos = devices.Select(device => new DeviceDto
        {
            SerialNumber = device.SerialNumber,
            Model = device.Model,
            EateryId = device.Eatery.Id,
        }).ToList();

        return devicesDtos;
    }
    
    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<DeviceDto>> GetById(int id)
    {
        var result = await _deviceRepository.GetByIdAsync(id);

        if (result is null)
            return BadRequest("Device not found");
        
        return new DeviceDto
        {
            SerialNumber = result.SerialNumber,
            Model = result.Model,
            EateryId = result.Eatery.Id
        };
    }
    
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Create([FromBody] DeviceDto dto)
    {
        var eatery = await _eateryRepository.GetByIdAsync(dto.EateryId);
        
        if (eatery is null)
            return BadRequest("Eatery not found");
        
        var device = new Device
        {
            SerialNumber = dto.SerialNumber,
            Model = dto.Model,
            Eatery = eatery
        };
        
        await _deviceRepository.CreateAsync(device);
        
        return CreatedAtAction(nameof(GetById), new { id = device.Id }, dto);
    }
    
    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult> Update(int id, [FromBody] DeviceDto dto)
    {
        var existingDevice = await _deviceRepository.GetByIdAsync(id);

        if (existingDevice == null)
            return BadRequest("Device not found");

        var eatery = await _eateryRepository.GetByIdAsync(dto.EateryId);
        if (eatery == null)
            return BadRequest("Eatery not found");

        existingDevice.SerialNumber = dto.SerialNumber;
        existingDevice.Model = dto.Model;
        existingDevice.Eatery = eatery;

        await _deviceRepository.UpdateAsync(existingDevice);

        return Ok();
    }
    
    [HttpPut("{id:int}/{delay:int}")]
    [Authorize]
    public async Task<ActionResult> Update(int id, int delay)
    {
        if (delay <= 0)
            return BadRequest("Delay must be greater than 0");
        
        var existingDevice = await _deviceRepository.GetByIdAsync(id);
        
        if (existingDevice == null)
            return BadRequest("Device not found");

        existingDevice.SecondsDelay = delay;

        await _deviceRepository.UpdateAsync(existingDevice);

        return Ok();
    }
    
    [HttpGet("{id:int}/delay")]
    public async Task<ActionResult<int>> GetDelaySecondsById(int id)
    {
        var result = await _deviceRepository.GetByIdAsync(id);

        if (result is null)
            return BadRequest("Device not found");
        
        return result.SecondsDelay;
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<ActionResult> Delete(int id)
    {
        await _deviceRepository.RemoveByIdAsync(id);
        
        return Ok();
    }
}