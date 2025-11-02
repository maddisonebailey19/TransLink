using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PrideLink.Client;
using PrideLink.Client.Helpers;
using System.Net.NetworkInformation;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

#if DEBUG
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7124") });// https://192.168.0.26:7125
#else
    // For production (live)
    builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://linkpride.duckdns.org/") });
#endif

builder.Services.AddScoped<jwtHelper>();
builder.Services.AddSingleton<LoginStatus>();
await builder.Build().RunAsync();