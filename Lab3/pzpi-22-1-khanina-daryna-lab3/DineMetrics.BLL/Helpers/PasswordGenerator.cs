using System.Text;

namespace DineMetrics.BLL.Helpers;

public static class PasswordGenerator
{
    private static readonly Random Random = new Random();

    public static string GenerateRandomPassword()
    {
        const int passwordLength = 8;
        const string upperCaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string lowerCaseChars = "abcdefghijklmnopqrstuvwxyz";
        const string numericChars = "0123456789";
        const string specialChars = "!@#$%^&*()-_=+";

        var passwordBuilder = new StringBuilder();
        passwordBuilder.Append(GetRandomChar(upperCaseChars));

        const string allChars = upperCaseChars + lowerCaseChars + numericChars + specialChars;
        while (passwordBuilder.Length < passwordLength)
        {
            passwordBuilder.Append(GetRandomChar(allChars));
        }

        return new string(passwordBuilder.ToString().OrderBy(_ => Random.Next()).ToArray());
    }

    private static char GetRandomChar(string availableChars)
    {
        return availableChars[Random.Next(availableChars.Length)];
    }
}