using DineMetrics.BLL.Services.Interfaces;
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
}