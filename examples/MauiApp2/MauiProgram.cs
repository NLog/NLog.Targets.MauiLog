using MauiApp2.ViewModel;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;

namespace MauiApp2;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var logger = NLog.LogManager.Setup().RegisterMauiLog()
			.LoadConfiguration(c => c.ForLogger().FilterMinLevel(NLog.LogLevel.Debug).WriteToMauiLog()) // Alternative use LoadConfigurationFromAssemblyResource()
            .GetCurrentClassLogger();

		try
		{
			var builder = MauiApp.CreateBuilder();

            // Add NLog for Logging
            builder.Logging.ClearProviders();
			builder.Logging.AddNLog();

            builder
				.UseMauiApp<App>()
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
					fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				});

			builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);

			builder.Services.AddSingleton<MainPage>();
			builder.Services.AddSingleton<MainViewModel>();


			builder.Services.AddTransient<DetailPage>();
			builder.Services.AddTransient<DetailViewModel>();

			return builder.Build();
		}
		catch (Exception ex)
		{
            logger.Error(ex, "Stopped program because of exception");
            throw;
        }
	}
}
