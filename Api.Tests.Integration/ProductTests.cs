using Application.Common.Interfaces.Services;
using Domain.Products;
using Microsoft.Extensions.DependencyInjection;
using Tests.Common;
using Tests.Data;
using Xunit;

namespace Api.Tests.Integration;

public class ProductTests : BaseTest, IAsyncLifetime
{
    private readonly IServiceScope _scope;
    private readonly IProductService _productService;
    
    private Product _product;
    public ProductTests(TestFactory factory) : base(factory)
    {
        _scope = factory.ServiceProvider.CreateScope();
        _productService = _scope.ServiceProvider.GetRequiredService<IProductService>();
        
    }

    public async Task InitializeAsync()
    {
        await Context.Products.AddAsync(ProductData.ProductOne);
        await Context.Products.AddAsync(ProductData.ProductTwo);
        _product = ProductData.ProductOne;
        await SaveChangesAsync();
    }

    public Task DisposeAsync()
    {
        Context.Products.RemoveRange(Context.Products);
        return SaveChangesAsync();
    }
    [Fact]
    public async Task GetProductById_Success()
    {
        var fetchedProduct = await _productService.GetById(_product.Id, CancellationToken.None);

        Assert.NotNull(fetchedProduct);
        Assert.Equal(_product.Id, fetchedProduct.Id);
    }

    [Fact]
    public async Task CreateProduct_Success()
    {
        var newProduct = await _productService.Add("New Product", 30.00m, CancellationToken.None);

        Assert.NotNull(newProduct);
        Assert.Equal("New Product", newProduct.Name);
        Assert.Equal(30.00m, newProduct.Price);
    }

    [Fact]
    public async Task UpdateProduct_Success()
    {
        var updatedProduct = await _productService.Update(_product.Id, "Updated Product", 40.00m, CancellationToken.None);

        Assert.NotNull(updatedProduct);
        Assert.Equal("Updated Product", updatedProduct.Name);
        Assert.Equal(40.00m, updatedProduct.Price);
    }

    [Fact]
    public async Task DeleteProduct_Success()
    {
        var deletedProduct = await _productService.Delete(_product.Id, CancellationToken.None);

        Assert.NotNull(deletedProduct);
        Assert.Equal(_product.Id, deletedProduct.Id);
    }

    [Fact]
    public async Task GetAllProducts_Success()
    {
        var products = await _productService.GetAll(CancellationToken.None);

        Assert.NotNull(products);
        Assert.NotEmpty(products);
        Assert.Contains(products, p => p.Id == _product.Id);
    }
    
    [Fact]
    public async Task CreateProduct_Failure_InvalidData()
    {
        await Assert.ThrowsAsync<Exception>(async () =>
            await _productService.Add("", -10.00m, CancellationToken.None));
    }

    [Fact]
    public async Task DeleteProduct_Failure_ProductNotFound()
    {
        var productId = new ProductId(Guid.NewGuid());

        await Assert.ThrowsAsync<Exception>(async () =>
            await _productService.Delete(productId, CancellationToken.None));
    }

    [Fact]
    public async Task UpdateProduct_Failure_ProductNotFound()
    {
        var productId = new ProductId(Guid.NewGuid());

        await Assert.ThrowsAsync<Exception>(async () =>
            await _productService.Update(productId, "Nonexistent Product", 50.00m, CancellationToken.None));
    }

    [Fact]
    public async Task GetProductById_Failure_ProductNotFound()
    {
        var productId = new ProductId(Guid.NewGuid());

        await Assert.ThrowsAsync<Exception>(async () =>
            await _productService.GetById(productId, CancellationToken.None));
    }
}