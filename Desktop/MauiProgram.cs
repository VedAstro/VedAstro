using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Storage;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Website;

namespace Desktop
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            //enable dialog to save file
            builder.Services.AddSingleton<IFileSaver>(FileSaver.Default);

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif


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



            //The last thing that the Main method does is to take all of the configuration
            //specified with the WebAssemblyHostBuilder and call its Build method. This will
            //create an instance of a WebAssemblyHost which is the heart of the Blazor app.
            //It contains all of the application configuration and services needed to run your app.
            var webAssemblyHost = builder.Build();


            //make the JS runtime globally accessible
            var jsRuntime = webAssemblyHost.Services.GetRequiredService<IJSRuntime>();
            AppData.JsRuntime = jsRuntime;

            //make wrapping for BLAZOR navigation easy, also overrides 
            var navigation = webAssemblyHost.Services.GetRequiredService<NavigationManager>();
            AppData.Navigation = navigation;


            return webAssemblyHost;
        }
    }
}
