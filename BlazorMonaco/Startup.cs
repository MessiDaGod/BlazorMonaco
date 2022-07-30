// joeshakely
using System;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.AspNetCore.Hosting;
using System.Globalization;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Stl.DependencyInjection;
using Stl.Fusion;
using Stl.Fusion.Authentication;
using Stl.Fusion.Blazor;
using Stl.Fusion.Bridge;
using Stl.Fusion.Client;
using Stl.IO;
using Stl.Fusion.Extensions;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Stl.Fusion.Operations.Reprocessing;
using Microsoft.Data.Sqlite;

namespace BlazorMonaco;

public class Startup
{
    private IConfiguration Cfg { get; }
    private ILogger Log { get; set; } = NullLogger<Startup>.Instance;

    public Startup(IConfiguration configuration)
    {
        Cfg = configuration;
    }
    public void ConfigureServices(IServiceCollection services)
    {


        // Logging
        services.AddLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddConsole();
            logging.SetMinimumLevel(LogLevel.None);

            logging.AddFilter("Microsoft", LogLevel.None);
            logging.AddFilter("Microsoft.AspNetCore.Hosting", LogLevel.None);
            logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.None);
            logging.AddFilter("Stl.Fusion.Operations", LogLevel.None);
            logging.AddFilter("Stl.CommandR", LogLevel.None);
            logging.AddFilter("System.Private.CoreLib", LogLevel.None);
        });

        var tmpServices = services.BuildServiceProvider();
        Log = tmpServices.GetRequiredService<ILogger<Startup>>();

        // DbContext & related services
        var appDir = FilePath.GetApplicationDirectory();
        var dbPath = appDir & "YahooApi.db";

        var fusion = services.AddFusion();
        var fusionClient = fusion.AddRestEaseClient();
        var shakely = services.AddFusion();
        var shakelyClient = shakely.AddRestEaseClient();

        // fusion.AddComputeService<SessionProvider, ISessionProvider>();

        fusion.AddSandboxedKeyValueStore();
        fusion.AddOperationReprocessor();

        CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");

        // Web
        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            options.KnownNetworks.Clear();
            options.KnownProxies.Clear();
        });
    }
}
