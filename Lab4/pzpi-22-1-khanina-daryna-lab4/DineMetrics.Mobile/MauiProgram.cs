using DineMetrics.Mobile.Pages;
using DineMetrics.Mobile.State.Authenticators;
using DineMetrics.Mobile.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;

namespace DineMetrics.Mobile;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		
		builder.Services.AddHttpClient();
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<MetricsViewModel>();
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<MetricsPage>();
        builder.Services.AddSingleton<IAuthenticator, Authenticator>();

        builder
            .UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
