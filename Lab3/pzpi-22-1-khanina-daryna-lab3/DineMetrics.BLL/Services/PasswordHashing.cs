using System.Security.Cryptography;
using System.Text;
using DineMetrics.BLL.Services.Interfaces;

namespace DineMetrics.BLL.Services;

public class PasswordHashing : IPasswordHashing
{
    public string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        
        var passwordBytes = Encoding.UTF8.GetBytes(password);
        var hashBytes = sha256.ComputeHash(passwordBytes);
            
        var builder = new StringBuilder();
        foreach (var t in hashBytes)
        {
            builder.Append(t.ToString("x2"));
        }

        return builder.ToString();
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        var hashedInput = HashPassword(password);
        return string.Equals(hashedInput, hashedPassword);
    }
}