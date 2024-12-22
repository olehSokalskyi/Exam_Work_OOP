using Application.Common.Interfaces;
using Application.Common.Interfaces.Builders;
using Application.Common.Interfaces.Repositories;
using Application.Orders;
using Domain.Orders;
using Domain.Products;
using Microsoft.Extensions.DependencyInjection;
using Tests.Common;
using Tests.Data;
using Xunit;

namespace Api.Tests.Integration;

public class OrderTests : BaseTest, IAsyncLifetime
{
    private readonly IOrderService _orderService;
    private readonly IServiceScope _scope;
    private IOrderBuilder _orderBuilder;
    private Order _order;

    public OrderTests(TestFactory factory) : base(factory)
    {
        _scope = factory.ServiceProvider.CreateScope();
        _orderService = _scope.ServiceProvider.GetRequiredService<IOrderService>();
        _orderBuilder = _scope.ServiceProvider.GetRequiredService<IOrderBuilder>();
    }

    public async Task InitializeAsync()
    {
        
        await Context.Products.AddAsync(ProductData.ProductOne);
        await Context.Products.AddAsync(ProductData.ProductTwo);
        
        var order = _orderBuilder.AddProduct(ProductData.ProductTwo).CalculateTotal().Build();
        _order = order;
        await Context.Orders.AddAsync(order);

        await SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        Context.Products.RemoveRange(Context.Products);
        await SaveChangesAsync();
    }
    [Fact]
    public async Task GetOrderById_Success()
    {
        
        var fetchedOrder = await _orderService.GetById(_order.Id, CancellationToken.None);

        Assert.NotNull(fetchedOrder);
        Assert.Equal(_order.Id, fetchedOrder.Id);
    }
    [Fact]
    public async Task CreateOrder_Success()
    {
        var data =  Context.Products.ToList();
        var products = new List<ProductId> { new ProductId(Guid.Parse("1ac9ff90-2d7e-43cc-bb81-28045c67cf56")) };
        var order = await _orderService.Add(products, CancellationToken.None);

        Assert.NotNull(order);
        Assert.Equal(1, order.Products.Count);
    }

    [Fact]
    public async Task CreateOrder_Failure_ProductNotFound()
    {
        var products = new List<ProductId> { new ProductId(Guid.NewGuid()) };

        await Assert.ThrowsAsync<ArgumentException>(async () =>
            await _orderService.Add(products, CancellationToken.None));
    }
    
    [Fact]
    public async Task DeleteOrder_Success()
    {
        
        var deletedOrder = await _orderService.Delete(_order.Id, CancellationToken.None);

        Assert.NotNull(deletedOrder);
        Assert.Equal(_order.Id, deletedOrder.Id);
    }
    
    [Fact]
    public async Task DeleteOrder_Failure_OrderNotFound()
    {
        var orderId = new OrderId(Guid.NewGuid());

        await Assert.ThrowsAsync<ArgumentException>(async () =>
            await _orderService.Delete(orderId, CancellationToken.None));
    }
    
    [Fact]
    public async Task GetOrderById_Failure_OrderNotFound()
    {
        var orderId = new OrderId(Guid.NewGuid());

        await Assert.ThrowsAsync<ArgumentException>(async () =>
            await _orderService.GetById(orderId, CancellationToken.None));
    }
    
    [Fact]
    public async Task GetAllOrders_Success()
    {
        var orders = await _orderService.GetAll(CancellationToken.None);

        Assert.NotNull(orders);
        Assert.NotEmpty(orders);
        Assert.True(orders.Any(o => o.Id == _order.Id), "The order list does not contain the expected order.");
    }
}