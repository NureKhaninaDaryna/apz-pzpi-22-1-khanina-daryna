using DineMetrics.BLL.Services;
using DineMetrics.BLL.Services.Interfaces;
using DineMetrics.Core.Shared;
using DineMetrics.DAL;
using DineMetrics.DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DeniMetrics.WebAPI.Configurations;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<DbContext, AppDbContext>();
        
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer("data source=localhost;Database=DineMetricsDb;TrustServerCertificate=true;Integrated Security=True;");
            options.UseLazyLoadingProxies();
        });
        
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        
        return services;
    }
    
    public static IServiceCollection AddCustomIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IPasswordHashing, PasswordHashing>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IAnalyticsService, AnalyticsService>();
        services.AddScoped<IEateryService, EateryService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IMetricService, MetricService>();
        services.AddScoped<TokenService>();

        services.Configure<AppSettings>(configuration.GetSection("AppSettings"));

        return services;
    }
}