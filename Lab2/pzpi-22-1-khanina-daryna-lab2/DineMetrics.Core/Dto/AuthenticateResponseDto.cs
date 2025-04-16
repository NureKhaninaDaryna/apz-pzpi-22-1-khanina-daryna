using DineMetrics.Core.Models;

namespace DineMetrics.Core.Dto;

public class AuthenticateResponseDto
{
    public AuthenticateResponseDto(User user, string token)
    {
        Id = user.Id;
        Email = user.Email;
        Token = token;
    }
    
    public int Id { get; set; }
    public string Email { get; set; }
    public string Token { get; set; }
}