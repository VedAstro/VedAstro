using Desktop.Data;
using Microsoft.Extensions.Logging;

namespace Desktop
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
    		builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton<WeatherForecastService>();


            //the client made here is used via AppData everywhere
            var httpClient = new HttpClient
            {
                //wait forever
                Timeout = new TimeSpan(0, 0, 0, 0, Timeout.Infinite),
                BaseAddress = new Uri("https://vedastro.org"),
                //DefaultRequestHeaders = { ConnectionClose = false } //keep alive
            };

            //specify to use TLS 1.2 as default connection
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            builder.Services.AddScoped(sp => httpClient);


            //FOR F12 Debug console
            builder.Services.AddBlazorWebViewDeveloperTools();

            return builder.Build();
        }
    }
}
