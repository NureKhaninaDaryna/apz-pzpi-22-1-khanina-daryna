namespace DineMetrics.BLL.Services.Interfaces;

public interface IPasswordHashing
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hashedPassword);
}