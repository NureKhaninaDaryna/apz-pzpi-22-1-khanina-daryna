using DeniMetrics.WebAPI.Attributes;
using DineMetrics.BLL.Helpers;
using DineMetrics.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DeniMetrics.WebAPI.Controllers;

[AuthorizeAsAdmin]
public class AdminsController : BaseController
{
    private readonly IEateryService _eateryService;
    private readonly IAuthenticationService _authenticationService;
    private readonly IEmailService _emailService;

    public AdminsController(
        IEateryService eateryService,
        IAuthenticationService authenticationService,
        IEmailService emailService)
    {
        _eateryService = eateryService;
        _authenticationService = authenticationService;
        _emailService = emailService;
    }

    [HttpPut("{id}/operating-hours")]
    public async Task<IActionResult> UpdateOperatingHours(int id, [FromBody] UpdateOperatingHoursDto dto)
    {
        var result = await _eateryService.UpdateOperatingHours(id, dto.From, dto.To);

        if (!result.IsSuccess)
            return BadRequest(result.Error.Message);

        return Ok(result.Value);
    }

    [HttpPut("{id}/maximum-capacity")]
    public async Task<IActionResult> UpdateMaximumCapacity(int id, [FromBody] UpdateMaximumCapacityDto dto)
    {
        var result = await _eateryService.UpdateMaximumCapacity(id, dto.Capacity);

        if (!result.IsSuccess)
            return BadRequest(result.Error.Message);

        return Ok(result.Value);
    }

    [HttpPut("{id}/temperature-threshold")]
    public async Task<IActionResult> UpdateTemperatureThreshold(int id, [FromBody] UpdateTemperatureThresholdDto dto)
    {
        var result = await _eateryService.UpdateTemperatureThreshold(id, dto.MinTemperature);

        if (!result.IsSuccess)
            return BadRequest(result.Error.Message);

        return Ok(result.Value);
    }
    
    [HttpPost("register-admin")]
    public async Task<IActionResult> RegisterNewAdmin([FromBody] AdminRequest dto)
    {
        var password = PasswordGenerator.GenerateRandomPassword();
        
        var userResult = await _authenticationService.Register(dto.Email, password, true);

        return !userResult.IsSuccess 
            ? BadRequest(userResult.Error.Message) 
            : HandleServiceResult(await _emailService.SendEmailAsync(dto.Email, "DyneMetrics", $"Your password: {password}"));
    }
}

public class UpdateOperatingHoursDto
{
    public string From { get; set; }
    public string To { get; set; }
}

public class UpdateMaximumCapacityDto
{
    public int Capacity { get; set; }
}

public class UpdateTemperatureThresholdDto
{
    public double MinTemperature { get; set; }
}

public class AdminRequest
{
    public string Email { get; set; }
}