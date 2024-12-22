using Application.Builder;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Builders;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Application.ConsoleWrapper;
using Application.Orders;
using Application.Products;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ConfigureApplication
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddSingleton<IOrderBuilder, OrderBuilder>();
        services.AddSingleton<IOrderNotifier, OrderNotifier>();
        services.AddSingleton<IConsoleWrapper, ConsoleWrapper.ConsoleWrapper>();
        services.AddScoped<IProductService, ProductService>(); 

        services.AddScoped<IOrderService, OrderService>(provider =>
        {
            var orderRepository = provider.GetRequiredService<IOrderRepository>();
            var logger = provider.GetRequiredService<ILogger>();
            var orderBuilder = provider.GetRequiredService<IOrderBuilder>();
            var productRepository = provider.GetRequiredService<IProductRepository>();
            var orderNotifier = provider.GetRequiredService<IOrderNotifier>();

            return new OrderService(orderRepository, logger, orderBuilder, productRepository, orderNotifier);
        });
    }
}