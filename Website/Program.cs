using AspNetMonsters.Blazor.Geolocation;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Website;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

//setup for global variables (to remember state)
builder.Services.AddScoped<GlobalVariableManager>();

//setup for getting location from browser
builder.Services.AddSingleton<LocationService>();

await builder.Build().RunAsync();
