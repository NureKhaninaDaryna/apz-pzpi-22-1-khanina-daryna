﻿using DeniMetrics.WebAPI.Attributes;
using DineMetrics.BLL.Services;
using DineMetrics.BLL.Services.Interfaces;
using DineMetrics.Core.Dto;
using DineMetrics.Core.Enums;
using DineMetrics.Core.Models;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace DeniMetrics.WebAPI.Controllers;

public class UsersController : BaseController
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IUserService _userService;
    private readonly TokenService _tokenService;

    public UsersController(IAuthenticationService authenticationService, IUserService userService, TokenService tokenService)
    {
        _authenticationService = authenticationService;
        _userService = userService;
        _tokenService = tokenService;
    }

    [HttpGet]
    public ActionResult<AuthenticateResponseDto> GetCurrentUser()
    {
        var currentUser = (User?)HttpContext.Items["User"];
        if (currentUser == null) return Unauthorized();
        
        var token = _tokenService.CreateToken(currentUser);
        
        return new AuthenticateResponseDto(currentUser, token, currentUser.Role);
    }

    [HttpGet("all")]
    [PermissionAuthorize(ManagementName.UsersManagement, PermissionAccess.Full)]
    public async Task<ActionResult<List<UserWithIdDto>>> GetAllUsers()
    {
        return await _userService.GetUsers();
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

    [HttpPatch("update-role")]
    [Authorize]
    [PermissionAuthorize(ManagementName.UsersManagement, PermissionAccess.Full)]
    public async Task<ActionResult<UserDto>> UpdateRole([FromBody] UpdateRoleDto dto)
    {
        var currentUser = (User?)HttpContext.Items["User"];
        
        if (currentUser?.Id == dto.Id) return BadRequest("You cannot change yourself role");
        
        var user = await _userService.ChangeRole(dto.Id, dto.Role);
        
        if (user == null) return BadRequest("User not found");
        
        return user;
    }
}

public class ChangePasswordRequest
{
    public string CurrentPassword { get; set; }
    public string NewPassword { get; set; }
}

public class UpdateRoleDto
{
    public int Id { get; set; }
    public UserRole Role { get; set; }
}