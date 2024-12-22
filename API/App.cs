using Application.Common.Interfaces;
using Application.Common.Interfaces.Services;
using Application.ConsoleWrapper;
using Application.Orders;
using Domain.Orders;
using Domain.Products;
using Microsoft.Extensions.DependencyInjection;

public enum MenuOptions
{
    GetAllOrders = 1,
    AddOrder = 2,
    GetOrderById = 3,
    UpdateOrder = 4,
    DeleteOrder = 5,
    GetAllProducts = 6,
    AddProduct = 7,
    GetProductById = 8,
    UpdateProduct = 9,
    DeleteProduct = 10,
    Exit = 0
}
public class App
{
    private readonly IOrderService _orderService;
    private readonly IProductService _productService;
    private readonly IConsoleWrapper _consoleWrapper;
    private readonly IOrderNotifier _orderNotifier;
    private readonly ILogger _logger;

    public App(IServiceProvider serviceProvider)
    {
        _orderService = serviceProvider.GetRequiredService<IOrderService>();
        _productService = serviceProvider.GetRequiredService<IProductService>();
        _consoleWrapper = serviceProvider.GetRequiredService<IConsoleWrapper>();
        _orderNotifier = serviceProvider.GetRequiredService<IOrderNotifier>();
        _logger = serviceProvider.GetRequiredService<ILogger>();
    }

    public async Task RunAsync(CancellationToken cancellationToken)
    {
        while (true)
        {
            _consoleWrapper.WriteLine("Select an option:");
            _consoleWrapper.WriteLine("1. Get all orders");
            _consoleWrapper.WriteLine("2. Add order");
            _consoleWrapper.WriteLine("3. Get order by ID");
            //_consoleWrapper.WriteLine("4. Update order");
            _consoleWrapper.WriteLine("5. Delete order");
            _consoleWrapper.WriteLine("6. Get all products");
            _consoleWrapper.WriteLine("7. Add product");
            _consoleWrapper.WriteLine("8. Get product by ID");
            _consoleWrapper.WriteLine("9. Update product");
            _consoleWrapper.WriteLine("10. Delete product");
            _consoleWrapper.WriteLine("0. Exit");

            var choice = _consoleWrapper.ReadLine();

            switch ((MenuOptions)Enum.Parse(typeof(MenuOptions), choice))
            {
                case MenuOptions.GetAllOrders:
                    await GetAllOrders(cancellationToken);
                    break;
                case MenuOptions.AddOrder:
                    await AddOrder(cancellationToken);
                    break;
                case MenuOptions.GetOrderById:
                    await GetOrderById(cancellationToken);
                    break;
                // case MenuOptions.UpdateOrder:
                //     await UpdateOrder(cancellationToken);
                //     break;
                case MenuOptions.DeleteOrder:
                    await DeleteOrder(cancellationToken);
                    break;
                case MenuOptions.GetAllProducts:
                    await GetAllProducts(cancellationToken);
                    break;
                case MenuOptions.AddProduct:
                    await AddProduct(cancellationToken);
                    break;
                case MenuOptions.GetProductById:
                    await GetProductById(cancellationToken);
                    break;
                case MenuOptions.UpdateProduct:
                    await UpdateProduct(cancellationToken);
                    break;
                case MenuOptions.DeleteProduct:
                    await DeleteProduct(cancellationToken);
                    break;
                case MenuOptions.Exit:
                    return;
                default:
                    _consoleWrapper.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }

    private async Task GetAllOrders(CancellationToken cancellationToken)
    {
        var orders = await _orderService.GetAll(cancellationToken);
        foreach (var ord in orders)
        {
            _consoleWrapper.WriteLine($"Order ID: {ord.Id}");
        }
    }

    private async Task AddOrder(CancellationToken cancellationToken)
    {
        _consoleWrapper.WriteLine("Enter product IDs (comma separated):");
        var orderObserver = new OrderObserver(_logger);
        _orderNotifier.Attach(orderObserver);
        var productIdsInput = _consoleWrapper.ReadLine();
        var productIds = productIdsInput.Split(',').Select(id => new ProductId(Guid.Parse(id.Trim()))).ToList();
        var newOrder = await _orderService.Add(productIds, cancellationToken);
        _consoleWrapper.WriteLine($"Added order with ID: {newOrder.Id}");
    }

    private async Task GetOrderById(CancellationToken cancellationToken)
    {
        _consoleWrapper.WriteLine("Enter order ID:");
        var orderId = new OrderId(Guid.Parse(_consoleWrapper.ReadLine()));
        var order = await _orderService.GetById(orderId, cancellationToken);
        _consoleWrapper.WriteLine($"Order ID: {order.Id}");
    }

    private async Task UpdateOrder(CancellationToken cancellationToken)
    {
        _consoleWrapper.WriteLine("Enter order ID:");
        var updateOrderId = new OrderId(Guid.Parse(_consoleWrapper.ReadLine()));
        _consoleWrapper.WriteLine("Enter product IDs (comma separated):");
        var updateProductIdsInput = _consoleWrapper.ReadLine();
        var updateProductIds = updateProductIdsInput.Split(',').Select(id => new ProductId(Guid.Parse(id.Trim()))).ToList();
        var updatedOrder = await _orderService.Update(updateOrderId, updateProductIds, cancellationToken);
        _consoleWrapper.WriteLine($"Updated order with ID: {updatedOrder.Id}");
    }

    private async Task DeleteOrder(CancellationToken cancellationToken)
    {
        _consoleWrapper.WriteLine("Enter order ID:");
        var deleteOrderId = new OrderId(Guid.Parse(_consoleWrapper.ReadLine()));
        var deletedOrder = await _orderService.Delete(deleteOrderId, cancellationToken);
        _consoleWrapper.WriteLine($"Deleted order with ID: {deletedOrder.Id}");
    }

    private async Task GetAllProducts(CancellationToken cancellationToken)
    {
        var products = await _productService.GetAll(cancellationToken);
        foreach (var prod in products)
        {
            _consoleWrapper.WriteLine($"Product ID: {prod.Id}, Name: {prod.Name}, Price: {prod.Price}");
        }
    }

    private async Task AddProduct(CancellationToken cancellationToken)
    {
        _consoleWrapper.WriteLine("Enter product name:");
        var productName = _consoleWrapper.ReadLine();
        _consoleWrapper.WriteLine("Enter product price:");
        var productPrice = decimal.Parse(_consoleWrapper.ReadLine());
        var newProduct = await _productService.Add(productName, productPrice, cancellationToken);
        _consoleWrapper.WriteLine($"Added product with ID: {newProduct.Id}");
    }

    private async Task GetProductById(CancellationToken cancellationToken)
    {
        _consoleWrapper.WriteLine("Enter product ID:");
        var productId = new ProductId(Guid.Parse(_consoleWrapper.ReadLine()));
        var product = await _productService.GetById(productId, cancellationToken);
        _consoleWrapper.WriteLine($"Product ID: {product.Id}, Name: {product.Name}, Price: {product.Price}");
    }

    private async Task UpdateProduct(CancellationToken cancellationToken)
    {
        _consoleWrapper.WriteLine("Enter product ID:");
        var updateProductId = new ProductId(Guid.Parse(_consoleWrapper.ReadLine()));
        _consoleWrapper.WriteLine("Enter new product name:");
        var updateProductName = _consoleWrapper.ReadLine();
        _consoleWrapper.WriteLine("Enter new product price:");
        var updateProductPrice = decimal.Parse(_consoleWrapper.ReadLine());
        var updatedProduct = await _productService.Update(updateProductId, updateProductName, updateProductPrice, cancellationToken);
        _consoleWrapper.WriteLine($"Updated product with ID: {updatedProduct.Id}");
    }

    private async Task DeleteProduct(CancellationToken cancellationToken)
    {
        _consoleWrapper.WriteLine("Enter product ID:");
        var deleteProductId = new ProductId(Guid.Parse(_consoleWrapper.ReadLine()));
        var deletedProduct = await _productService.Delete(deleteProductId, cancellationToken);
        _consoleWrapper.WriteLine($"Deleted product with ID: {deletedProduct.Id}");
    }
}

