using DineMetrics.BLL.Services.Interfaces;
using DineMetrics.Core.Dto;
using DineMetrics.Core.Enums;
using DineMetrics.Core.Models;
using DineMetrics.Core.Shared;
using DineMetrics.DAL.Repositories;

namespace DineMetrics.BLL.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserService _userService;
    private readonly IPasswordHashing _passwordHashing;
    private readonly IJwtService _jwtService;
    private readonly IRepository<User> _userRepository;

    public AuthenticationService(
        IUserService userService, 
        IPasswordHashing passwordHashing,
        IJwtService jwtService,
        IRepository<User> userRepository)
    {
        _userService = userService;
        _passwordHashing = passwordHashing;
        _jwtService = jwtService;
        _userRepository = userRepository;
    }

    public async Task<ServiceResult<UserDto>> Register(string email, string password, bool isAdmin = false)
    {
        if (!await _userService.IsFreeEmail(email))
        {
            return ServiceResult<UserDto>.Failure(ServiceErrors.NotFreeEmail);
        }

        var passwordHash = _passwordHashing.HashPassword(password);

        var user = new User()
        {
            Email = email,
            PasswordHash = passwordHash,
            Role = isAdmin ? UserRole.Admin : UserRole.Manager,
        };
        
        var userDto = new UserDto()
        {
            Email = email
        };

        try
        {
            await _userRepository.CreateAsync(user);

            return ServiceResult<UserDto>.Success(userDto);
        }
        catch 
        {
            return ServiceResult<UserDto>.Failure(ServiceErrors.FailedRegistration);
        }
    }
    
    public async Task<ServiceResult<AuthenticateResponseDto>> Authenticate(string email, string password)
    {
        var user = await _userService.GetUserByEmail(email);

        if (user == null)
        {
            return ServiceResult<AuthenticateResponseDto>.Failure(ServiceErrors.FailedAuthenticateByEmail);
        }

        var verifyPasswordResult = _passwordHashing.VerifyPassword(password, user.PasswordHash);

        if (!verifyPasswordResult)
        {
            return ServiceResult<AuthenticateResponseDto>.Failure(ServiceErrors.FailedAuthenticateByPassword);
        }
        
        var token = _jwtService.GenerateJwtToken(user);

        return ServiceResult<AuthenticateResponseDto>.Success(new AuthenticateResponseDto(user, token));
    }

    public async Task<ServiceResult> ChangePassword(int userId, string currentPassword, string newPassword)
    {
        var user = await _userService.GetUserById(userId);

        if (user == null)
        {
            return ServiceResult<AuthenticateResponseDto>.Failure(ServiceErrors.FailedAuthenticateByEmail);
        }
        
        var verifyPasswordResult = _passwordHashing.VerifyPassword(currentPassword, user.PasswordHash);

        if (!verifyPasswordResult)
        {
            return ServiceResult<AuthenticateResponseDto>.Failure(ServiceErrors.FailedAuthenticateByPassword);
        }
        
        var passwordHash = _passwordHashing.HashPassword(newPassword);
        
        user.PasswordHash = passwordHash;
        
        try
        {
            await _userRepository.UpdateAsync(user);
            
            return ServiceResult.Success;
        }
        catch 
        {
            return ServiceResult.Failure(ServiceErrors.FailedChangePassword);
        }
    }
}