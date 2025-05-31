using CommunityToolkit.Mvvm.ComponentModel;
using DineMetrics.Core.Dto;
using DineMetrics.Core.Enums;
using System.Net.Http.Json;

namespace DineMetrics.Mobile.State.Authenticators
{
    public class Authenticator : ObservableObject, IAuthenticator
    {
        private readonly HttpClient _httpClient;

        private AuthenticateResponseDto? _currentUser;

        public AuthenticateResponseDto? CurrentUser
        {
            get => _currentUser;
            private set
            {
                _currentUser = value;
                OnPropertyChanged(nameof(CurrentUser));
                OnPropertyChanged(nameof(IsLoggedIn));
                OnPropertyChanged(nameof(Role));
            }
        }

        public bool IsLoggedIn => CurrentUser != null;

        public UserRole? Role => CurrentUser?.Role;

        public Authenticator(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("http://10.0.2.2:5048/");
        }

        public async Task<(bool, string?)> SignIn(string email, string password)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("users/login", new 
                {
                    Email = email,
                    Password = password
                });

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    return (false, error);
                }

                var result = await response.Content.ReadFromJsonAsync<AuthenticateResponseDto>();

                if (result == null)
                    return (false, "Authentication failed: User not found");

                if (result.Role != UserRole.Admin)
                    return (false, $"User {email} is not an administrator");
                
                CurrentUser = result;

                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, $"Error: {ex.Message}");
            }
        }

        public void LogOut()
        {
            CurrentUser = default;
        }
    }
}
