using DineMetrics.Core.Enums;
using DineMetrics.Core.Models;
using System.Text.Json.Serialization;

namespace DineMetrics.Core.Dto;

public class AuthenticateResponseDto
{
    public AuthenticateResponseDto()
    {
        
    }

    public AuthenticateResponseDto(User user, string token, UserRole role)
    {
        Id = user.Id;
        Email = user.Email;
        Token = token;
        Role = role;
    }
    
    public int Id { get; set; }
    public string Email { get; set; }
    public string Token { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public UserRole Role { get; set; }
}