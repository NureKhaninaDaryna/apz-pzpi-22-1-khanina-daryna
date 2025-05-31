using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DineMetrics.Mobile.State.Authenticators;

namespace DineMetrics.Mobile.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly IAuthenticator _authenticator;

        [ObservableProperty]
        private string username;

        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private string feedbackMessage;
        public ICommand LoginCommand { get; }

        public LoginViewModel(IAuthenticator authenticator)
        {
            _authenticator = authenticator;
            LoginCommand = new AsyncRelayCommand(OnLogin);
        }

        private async Task OnLogin()
        {
            var result = await _authenticator.SignIn(Username, Password);

            FeedbackMessage = !result.Item1 ? result.Item2! : "You successfully logged in!";
        }
    }
}
