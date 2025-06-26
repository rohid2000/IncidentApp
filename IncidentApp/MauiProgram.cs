using IncidentApp.ViewModels;
using Microsoft.Extensions.Logging;

namespace IncidentApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		builder.Services.AddTransient<MainPageViewModel>();
		builder.Services.AddTransient<RegisterPageViewModel>();
		builder.Services.AddTransient<LoginPageViewModel>();
		builder.Services.AddTransient<UserReportedIncidentPageViewModel>();

		builder.Services.AddTransient<MainPage>();
		builder.Services.AddTransient<RegisterPage>();
		builder.Services.AddTransient<LoginPage>();
		builder.Services.AddTransient<UserReportedIncidentsPage>();

#if DEBUG
		builder.Logging.AddDebug();
#endif
		builder.Services.AddSingleton<HttpClient>();

		return builder.Build();
	}
}
