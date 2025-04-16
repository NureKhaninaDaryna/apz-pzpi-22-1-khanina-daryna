using DineMetrics.Core.Enums;
using DineMetrics.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace DineMetrics.DAL.Helpers;

public static class ModelBuilderExtensions
{
    public static void Seed(this ModelBuilder modelBuilder)
    {
        var admin = new User()
        {
            Id = 1,
            Email = "admin@gmail.com",
            PasswordHash = "7f5b3331091e4db703d6d6ab8a8039ee4f11599f1d2386619b2b55c784361562",
            Role = UserRole.Admin,
            AppointmentDate = new DateOnly(2022, 11, 28)
        };
        
        modelBuilder.Entity<User>().HasData(admin);
    }
}