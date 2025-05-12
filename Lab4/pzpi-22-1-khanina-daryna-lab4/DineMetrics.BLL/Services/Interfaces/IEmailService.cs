using DineMetrics.Core.Enums;
using DineMetrics.Core.Shared;

namespace DineMetrics.BLL.Services.Interfaces;

public interface IEmailService
{
    Task<ServiceResult> SendEmailAsync(string email, string subject, string body);
}