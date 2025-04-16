using DineMetrics.BLL.Services.Interfaces;
using DineMetrics.Core.Enums;
using DineMetrics.Core.Models;
using DineMetrics.Core.Shared;
using DineMetrics.DAL.Repositories;

namespace DineMetrics.BLL.Services;

public class MetricService : IMetricService
{
    private readonly IRepository<CustomerMetric> _customerMetricRepository;
    private readonly IRepository<TemperatureMetric> _temperatureMetricRepository;
    private readonly IRepository<Report> _reportRepository;
    private readonly IRepository<User> _userRepository;
    private readonly IEmailService _emailService;

    public MetricService(
        IRepository<CustomerMetric> customerMetricRepository, 
        IRepository<TemperatureMetric> temperatureMetricRepository,
        IRepository<Report> reportRepository,
        IRepository<User> userRepository,
        IEmailService emailService)
    {
        _customerMetricRepository = customerMetricRepository;
        _temperatureMetricRepository = temperatureMetricRepository;
        _reportRepository = reportRepository;
        _userRepository = userRepository;
        _emailService = emailService;
    }
    
    public async Task<ServiceResult> CreateTemperatureMetric(TemperatureMetric metric)
    {
        var currentDate = DateOnly.FromDateTime(DateTime.Now);

        var todayReport = await _reportRepository.GetByPredicateAsync(r => r.ReportDate == currentDate);

        if (todayReport.Count > 0)
        {
            metric.Report = todayReport.FirstOrDefault()!;
            
            await UpdateReport(metric);
        }
        else
        {
            var report = new Report()
            {
                AverageTemperature = metric.Value,
                ReportDate = currentDate
            };

            await _reportRepository.CreateAsync(report);
            
            metric.Report = report;
        }

        await _temperatureMetricRepository.CreateAsync(metric);

        return ServiceResult.Success;
    }

    public async Task<ServiceResult> CreateCustomerMetric(CustomerMetric metric)
    {
        var currentDate = DateOnly.FromDateTime(DateTime.Now);

        var todayReport = await _reportRepository.GetByPredicateAsync(r => r.ReportDate == currentDate);
        
        if (todayReport.Count > 0)
        {
            metric.Report = todayReport.FirstOrDefault()!;

            await UpdateReport(metric);
        }
        else
        {
            if (metric.Count <= 0)
            {
                return ServiceResult.Success;
            }
            
            var report = new Report()
            {
                TotalCustomers = metric.Count,
                ReportDate = currentDate
            };

            await _reportRepository.CreateAsync(report);
            
            metric.Report = report;
        }

        await _customerMetricRepository.CreateAsync(metric);
        
        return ServiceResult.Success;
    }

    private async Task UpdateReport(TemperatureMetric metric)
    {
        var temp = metric.Report.AverageTemperature;
        temp += metric.Value;
        temp /= 2;
        
        metric.Report.AverageTemperature = temp;
        
        await _reportRepository.UpdateAsync(metric.Report);
        
        await CheckReport(metric.Report, temperatureMetric: metric);
    }
    
    private async Task UpdateReport(CustomerMetric metric)
    {
        var metrics = metric.Report.TotalCustomers;
        metrics += metric.Count;

        if (metrics < 0)
        {
            return;
        }
        
        metric.Report.TotalCustomers = metrics;
        
        await _reportRepository.UpdateAsync(metric.Report);
        
        await CheckReport(metric.Report, customerMetric: metric);
    }

    private async Task CheckReport(Report report, CustomerMetric? customerMetric = null, TemperatureMetric? temperatureMetric = null)
    {
        var eatery = customerMetric != null 
            ? customerMetric.Device.Eatery 
            : temperatureMetric!.Device.Eatery;

        
        if (customerMetric != null && eatery.MaximumCapacity < report.TotalCustomers)
        {
            var admins = await _userRepository.GetByPredicateAsync(u => u.Role == UserRole.Admin);

            foreach (var admin in admins)
            {
                await _emailService.SendEmailAsync(admin.Email, "Customer Report", $"Now at eatery: {eatery.Name}, customers: {report.TotalCustomers}. This more than: {eatery.MaximumCapacity} capacity.");
            }
        }

        if (temperatureMetric != null && eatery.TemperatureThreshold > report.AverageTemperature)
        {
            var admins = await _userRepository.GetByPredicateAsync(u => u.Role == UserRole.Admin);

            foreach (var admin in admins)
            {
                await _emailService.SendEmailAsync(admin.Email, "Temperature Report", $"Now at eatery: {eatery.Name}, temperature: +{report.AverageTemperature}. This less than: +{eatery.TemperatureThreshold}.");
            }
        }
    }
}