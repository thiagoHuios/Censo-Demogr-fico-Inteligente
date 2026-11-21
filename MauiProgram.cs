using IbgeDataInsights.Services;
using Microsoft.Extensions.Logging;

namespace IbgeDataInsights;

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

        // Registro da camada de Serviços
        builder.Services.AddSingleton<IbgeService>();

        // Registro da camada de Apresentação (Views)
        builder.Services.AddTransient<MainPage>();

        return builder.Build();
    }
}