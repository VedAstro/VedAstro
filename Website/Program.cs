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
//get the services needed and reference them in global var
globalVariable.JsRuntime = builder.Services.First((s) => s.ServiceType == typeof(IJSRuntime)).ImplementationInstance as IJSRuntime;
globalVariable.Navigation = builder.Services.First((s) => s.ServiceType == typeof(NavigationManager)).ImplementationInstance as NavigationManager;
builder.Services.AddSingleton(globalVariable);

//setup for getting location from browser
builder.Services.AddSingleton<LocationService>();


await builder.Build().RunAsync();




//ARCHIVED CODE

//foreach (var builderService in builder.Services)
//{
//    Console.WriteLine(builderService.ToString());
//}
