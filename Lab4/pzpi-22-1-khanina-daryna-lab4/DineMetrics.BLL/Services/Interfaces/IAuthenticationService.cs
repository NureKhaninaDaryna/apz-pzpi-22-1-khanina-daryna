using DineMetrics.Core.Dto;
using DineMetrics.Core.Shared;

namespace DineMetrics.BLL.Services.Interfaces;

public interface IAuthenticationService
{
    Task<ServiceResult<UserDto>> Register(string email, string password, bool isAdmin = false);
    Task<ServiceResult<AuthenticateResponseDto>> Authenticate(string email, string password);
    Task<ServiceResult> ChangePassword(int userId, string currentPassword, string newPassword);
}