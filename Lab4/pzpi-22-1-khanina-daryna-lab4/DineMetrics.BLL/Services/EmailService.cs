using System.Net;
using System.Net.Mail;
using DineMetrics.BLL.Services.Interfaces;
using DineMetrics.Core.Shared;
using Microsoft.Extensions.Configuration;

namespace DineMetrics.BLL.Services;

public class EmailService : IEmailService   
{
    private readonly MailAddress _fromAddress;
    private readonly string _password;

    public EmailService(IConfiguration configuration)
    {
        _fromAddress = new MailAddress(configuration["Email:Address"]!, configuration["Email:Name"]!);
        _password = configuration["Email:Password"]!;
    }
    
    public async Task<ServiceResult> SendEmailAsync(string email, string subject, string body)
    {
        try
        {
            var toAddress = new MailAddress(email);
        
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_fromAddress.Address, _password)
            };
        
            using var message = new MailMessage(_fromAddress, toAddress);
        
            message.Subject = subject;
            message.Body = body;
            smtp.Send(message);

            return ServiceResult.Success;
        }
        catch (Exception ex)
        {
            return ServiceResult.Failure(new Error(ex.Message));
        }
    }
}