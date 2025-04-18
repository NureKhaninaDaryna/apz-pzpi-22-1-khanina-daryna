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
            PasswordHash = "83fcd3f7129b081faeb043dc07262e63fea599da4be6869a7a1780f7084a15b4",
            Role = UserRole.Admin,
            AppointmentDate = new DateOnly(2022, 11, 28)
        };
        
        modelBuilder.Entity<User>().HasData(admin);
    }
}