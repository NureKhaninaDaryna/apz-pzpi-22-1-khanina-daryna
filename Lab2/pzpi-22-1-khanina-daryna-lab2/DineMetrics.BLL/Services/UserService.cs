using DineMetrics.BLL.Services.Interfaces;
using DineMetrics.Core.Dto;
using DineMetrics.Core.Enums;
using DineMetrics.Core.Models;
using DineMetrics.DAL.Repositories;

namespace DineMetrics.BLL.Services;

public class UserService : IUserService
{
    private readonly IRepository<User> _repository;

    public UserService(IRepository<User> repository)
    {
        _repository = repository;
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        var users = await _repository.GetByPredicateAsync(u => u.Email == email);

        return users.FirstOrDefault();
    }

    public async Task<User?> GetUserById(int id) => await _repository.GetByIdAsync(id);

    public async Task<bool> IsFreeEmail(string email) => await GetUserByEmail(email) == null;
    
    public async Task<UserDto?> ChangeRole(int userId, UserRole role)
    {
        var user = await GetUserById(userId);
        if (user == null)
        {
            return null;
        }
        
        user.Role = role;
        
        await _repository.UpdateAsync(user);
        
        return new UserDto { Email = user.Email, Role = user.Role };
    }
}