using DineMetrics.Core.Models;
using DineMetrics.DAL.Helpers;
using Microsoft.EntityFrameworkCore;

namespace DineMetrics.DAL;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    
    public DbSet<Eatery> Eateries { get; set; } = null!;
    
    public DbSet<Device> Devices { get; set; } = null!;
    
    public DbSet<TemperatureMetric> TemperatureMetrics { get; set; } = null!;
    
    public DbSet<CustomerMetric> CustomerMetrics { get; set; } = null!;
    
    public DbSet<Report> Reports { get; set; } = null!;
    
    public DbSet<Employee> Employees { get; set; } = null!;
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("data source=khanina-d;Database=DineMetricsDb;TrustServerCertificate=true;Integrated Security=True;");
        }
        
        base.OnConfiguring(optionsBuilder);
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Seed();
        
        base.OnModelCreating(modelBuilder);
    }
}