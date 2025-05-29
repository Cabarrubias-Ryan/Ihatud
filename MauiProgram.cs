using Microsoft.Extensions.Logging;
using Ihatud.Services;

namespace Ihatud;

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

#if DEBUG
		builder.Logging.AddDebug();
#endif
		string dbPath = Path.Combine(FileSystem.AppDataDirectory, "myapp.db");
		Console.WriteLine($"Database Path: {dbPath}");
        builder.Services.AddSingleton<DatabaseService>(s => new DatabaseService(dbPath));
		return builder.Build();
	}
}
