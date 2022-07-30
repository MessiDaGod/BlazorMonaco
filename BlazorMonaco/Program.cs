using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;

using Newtonsoft.Json;
using Microsoft.Extensions.Logging.Abstractions;
using MudBlazor.Services;
using Microsoft.Extensions.DependencyInjection;
using BlazorMonaco;
using System.Net.Http;
using System.IO;
using System;
using Stl.Fusion.Authentication;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Stl.Fusion.Blazor;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddMudServices();
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.TryAddScoped<ISessionProvider, SessionProvider>();
builder.Services.TryAddScoped<BlazorCircuitContext, BlazorCircuitContext>();
// var DbDir = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), "database"));
// var DatabasePath = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), "database"), "main.db");

await builder.Build().RunAsync();
