using AspNetMonsters.Blazor.Geolocation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using Website;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

//setup for global variables (to remember state)
var globalVariable = new GlobalVariableManager();
builder.Services.AddSingleton(globalVariable);

//setup for getting location from browser
builder.Services.AddSingleton<LocationService>();

//ERROR HANDLING

//hide generic error messages only in RELEASE
//when debugging enable default errors
//apps published will not show any error while
//same run locally will show messages
#if (DEBUG == false)
//stop default error handling (prints in console log & shows default error message at page bottom)
builder.Logging.ClearProviders();
#endif

//add custom error handling (synchronous exceptions only)
var unhandledExceptionSender = new UnhandledExceptionSender();
var unhandledExceptionProvider = new UnhandledExceptionProvider(unhandledExceptionSender);
builder.Logging.AddProvider(unhandledExceptionProvider);
builder.Services.AddSingleton<IUnhandledExceptionSender>(unhandledExceptionSender);


//The last thing that the Main method does is to take all of the configuration
//specified with the WebAssemblyHostBuilder and call its Build method. This will
//create an instance of a WebAssemblyHost which is the heart of your Blazor app.
//It contains all of the application configuration and services needed to run your app.
var webAssemblyHost = builder.Build();

await webAssemblyHost.RunAsync();




//ARCHIVED CODE

//foreach (var builderService in builder.Services)
//{
//    Console.WriteLine(builderService.ToString());
//}
