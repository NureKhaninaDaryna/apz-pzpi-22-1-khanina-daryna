using System.Text.Json.Serialization;
using DeniMetrics.WebAPI.Configurations;
using DeniMetrics.WebAPI.Middlewares;
using DeniMetrics.WebAPI.Validators;
using DineMetrics.BLL.Hubs;
using DineMetrics.DAL;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var builderConfig = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

var configuration = builderConfig.Build();

builder.Configuration.AddConfiguration(configuration);

// Add services to the container.
builder.Services
    .AddInfrastructure()
    .AddCustomIdentity(configuration);

builder.Services.AddSignalR();

// Add Swagger services
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "DeniMetrics API",
        Description = "API documentation for DeniMetrics",
        Contact = new OpenApiContact
        {
            Name = "Support",
            Email = "support@denimetrics.com"
        }
    });

    // Optionally, add security definition for JWT
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. Example: 'Authorization: Bearer {token}'",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = null;
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    })
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<RegisterRequestValidator>())
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<EateryDtoValidator>())
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CustomerMetricDtoValidator>())
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<DeviceDtoValidator>())
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<EmployeeDtoValidator>())
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<TemperatureMetricDtoValidator>())
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<AdminRequestValidator>());

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "DeniMetrics API v1");
        c.RoutePrefix = string.Empty; // Set Swagger at the root
    });
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Migration failed: {ex.Message}");
    }
}

app.UseHttpsRedirection();

app.UseCors("AllowReactApp");
app.UseCors("CorsPolicy");
app.UseAuthorization();

app.UseMiddleware<JwtMiddleware>();
app.UseMiddleware<ExceptionsHandlingMiddleware>();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapFallbackToFile("index.html");

app.MapControllers();

app.MapHub<NotificationHub>("/hub/notifications");

app.Run();
