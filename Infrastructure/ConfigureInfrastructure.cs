using Application.Common.Interfaces;
using Implementation.Logger;
using Implementation.Persistance;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Implementation;

public static class ConfigureInfrastructure
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ILoggerFactory, LoggerFactory>();
        services.AddSingleton(provider => provider.GetRequiredService<ILoggerFactory>().CreateLogger());
        services.AddPersistence(configuration);
    }
}