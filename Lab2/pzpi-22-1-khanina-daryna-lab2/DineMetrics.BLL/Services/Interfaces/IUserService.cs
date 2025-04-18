using DineMetrics.Core.Dto;
using DineMetrics.Core.Enums;
using DineMetrics.Core.Models;

namespace DineMetrics.BLL.Services.Interfaces;

public interface IUserService
{
    Task<User?> GetUserByEmail(string email);
    Task<User?> GetUserById(int id);
    Task<bool> IsFreeEmail(string email);
    Task<UserDto?> ChangeRole(int userId, UserRole role);
}