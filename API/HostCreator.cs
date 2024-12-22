using Application;
using Implementation;
using Implementation.Persistance;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API;

public static class HostCreator
{
    public static async Task<IHost> CreateHost(string[] args, Action<IHostBuilder>? configureHost = null)
    {
        string directoryPath = AppDomain.CurrentDomain.BaseDirectory;
        string jsonFileName = Directory.GetFiles(directoryPath, "*.json").First();

        var builder = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile(jsonFileName, optional: false, reloadOnChange: true);
            })
            .ConfigureLogging(logging => { logging.ClearProviders(); })
            .ConfigureServices((context, services) =>
            {
                var configuration = context.Configuration;

                services.AddInfrastructure(configuration);
                
                services.AddApplicationServices();
            });

        configureHost?.Invoke(builder);

        var host = builder.Build();

        using var scope = host.Services.CreateScope();
        var dbContextInitializer = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContextInitialiser>();
        await dbContextInitializer.InitializeAsync();

        return host;
    }
}