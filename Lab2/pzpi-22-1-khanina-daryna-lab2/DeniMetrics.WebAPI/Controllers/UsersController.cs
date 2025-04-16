using DeniMetrics.WebAPI.Attributes;
using DineMetrics.BLL.Services;
using DineMetrics.BLL.Services.Interfaces;
using DineMetrics.Core.Dto;
using DineMetrics.Core.Models;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace DeniMetrics.WebAPI.Controllers;

public class UsersController : BaseController
{
    private readonly IAuthenticationService _authenticationService;
    private readonly TokenService _tokenService;

    public UsersController(IAuthenticationService authenticationService, TokenService tokenService)
    {
        _authenticationService = authenticationService;
        _tokenService = tokenService;
    }

    [HttpGet]
    public ActionResult<AuthenticateResponseDto> GetCurrentUser()
    {
        var currentUser = (User?)HttpContext.Items["User"];
        if (currentUser == null) return Unauthorized();
        
        var token = _tokenService.CreateToken(currentUser);
        
        return new AuthenticateResponseDto(currentUser, token);
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register([FromBody] RegisterRequest model)
    {
        return HandleServiceResult(await _authenticationService.Register(model.Email, model.Password));
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthenticateResponseDto>> Login([FromBody] RegisterRequest model)
    {
        return HandleServiceResult(await _authenticationService.Authenticate(model.Email, model.Password));
    }
    
    [HttpPost("change-password")]
    [Authorize]
    public async Task<ActionResult<AuthenticateResponseDto>> ChangePassword([FromBody] ChangePasswordRequest model)
    {
        return HandleServiceResult(await _authenticationService.ChangePassword(CurrentUser!.Id ,model.CurrentPassword, model.NewPassword));
    }
}

public class ChangePasswordRequest
{
    public string CurrentPassword { get; set; }
    public string NewPassword { get; set; }
}