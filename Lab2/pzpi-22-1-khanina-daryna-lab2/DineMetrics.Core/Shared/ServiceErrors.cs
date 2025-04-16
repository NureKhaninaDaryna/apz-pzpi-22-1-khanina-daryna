namespace DineMetrics.Core.Shared;

public class ServiceErrors
{
    public static readonly Error NotFreeEmail = new("This email is already used in the system");
    public static readonly Error FailedRegistration = new("Failed to register");
    public static readonly Error FailedChangePassword = new("Failed to change password");
    public static readonly Error JwtSecretNotConfigured = new("JWT secret not configured");
    public static readonly Error FailedAuthenticateByEmail = new("Failed to sign in the system with incorrect email");
    public static readonly Error FailedAuthenticateByPassword = new("Failed to sign in the system with incorrect password");
}