using DineMetrics.Core.Dto;
using DineMetrics.Core.Models;

namespace DineMetrics.Mobile.State.Authenticators
{
    public interface IAuthenticator
    {
        AuthenticateResponseDto? CurrentUser { get; }

        bool IsLoggedIn { get; }

        Task<(bool, string?)> SignIn(string email, string password);

        void LogOut();
    }
}
