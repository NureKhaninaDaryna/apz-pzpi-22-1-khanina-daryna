using System.Linq.Expressions;
using DineMetrics.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace DineMetrics.DAL.Repositories;

public class Repository<T> : IRepository<T> where T : BaseEntity
{
    private readonly DbContext _context;

    private readonly DbSet<T> _dbSet;

    public Repository(DbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public Task<T?> GetByIdAsync(int id)
    {
        return _dbSet.FirstOrDefaultAsync(u => u.Id.Equals(id));
    }

    public Task<List<T>> GetByPredicateAsync(Expression<Func<T, bool>> predicate)
    {
        return _dbSet
            .Where(predicate)
            .ToListAsync();
    }

    public Task<List<T>> GetAllAsync() => _dbSet.ToListAsync();

    public async Task CreateAsync(T item)
    {
        await _dbSet.AddAsync(item);
        await _context.SaveChangesAsync();
    }

    public Task UpdateAsync(T item)
    {
        _dbSet.Update(item);

        return _context.SaveChangesAsync();
    }
    
    public async Task RemoveByIdAsync(int id)
    {
        var itemToRemove = await GetByIdAsync(id);

        if (itemToRemove != null)
        {
            _context.Remove(itemToRemove);
        }

        await _context.SaveChangesAsync();
    }

    public List<T> GetAll()  => _dbSet.ToList();
    
    public List<T> GetByPredicate(Expression<Func<T, bool>> predicate)
    {
        return _dbSet
            .Where(predicate)
            .ToList();
    }
}