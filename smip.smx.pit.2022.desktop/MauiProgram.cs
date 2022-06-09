using Microsoft.AspNetCore.Components.WebView.Maui;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using smip.smx.pit._2022.desktop.Data;
using smip.smx.pit._2022.webapp.Data;
using System.Reflection;

namespace smip.smx.pit._2022.desktop
{
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
                });

            builder.Services.AddMauiBlazorWebView();
#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
#endif

            builder.Services.AddSingleton<WeatherForecastService>();



            var a = Assembly.GetExecutingAssembly();
            using var stream = a.GetManifestResourceStream("smip.smx.pit._2022.desktop.appsettings.json");

            var config = new ConfigurationBuilder()
                        .AddJsonStream(stream)
                        .Build();

            builder.Configuration.AddConfiguration(config);


            builder.Services.AddSingleton<SmipService>();

            return builder.Build();
        }
    }
}