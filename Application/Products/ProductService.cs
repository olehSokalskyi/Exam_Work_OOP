using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Domain.Products;
using Microsoft.Extensions.Logging;
using ILogger = Application.Common.Interfaces.ILogger;

namespace Application.Products;

public class ProductService(ILogger logger, IProductRepository productRepository): IProductService
{
    public async Task<IReadOnlyList<Product>> GetAll(CancellationToken cancellationToken)
    {
        var products = await productRepository.GetAll(cancellationToken);
        logger.LogInformation("GetAll products");
        return products;
    }

    public async Task<Product> Add(string name, decimal price, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                logger.LogInformation("Product name is required");
                throw new ArgumentException("Product name is required");
            }
            var product = Product.New(ProductId.New(), name, price);
            var newProduct = await productRepository.Add(product, cancellationToken);
            logger.LogInformation($"Add product with id {newProduct.Id}");
            return newProduct;    
        }
        catch (Exception e)
        {
            logger.LogInformation($"Error adding product: {e.Message}");
            throw;
        }
        
    }

    public async Task<Product> GetById(ProductId id, CancellationToken cancellationToken)
    {
        try
        {
            var product = await productRepository.GetById(id, cancellationToken);
            if(product is null)
            {
                logger.LogInformation($"Product with id {id} not found");
                throw new ArgumentException("Product not found");
            }
            logger.LogInformation($"Get product with id {id}");
            return product;
        }
        catch (Exception e)
        {
            logger.LogInformation($"Error getting product: {e.Message}");
            throw;
        }
    }

    public async Task<Product> Update(ProductId productId, string name, decimal price, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                logger.LogInformation("Product name is required");
                throw new ArgumentException("Product name is required");
            }
            var product = await productRepository.GetById(productId, cancellationToken);
            if (product is null)
            {
                logger.LogInformation($"Product with id {productId} not found");
                throw new ArgumentException("Product not found");
            }
            product.UpdateDetails(name,price);
            var updatedProduct = await productRepository.Update(product, cancellationToken);
            logger.LogInformation($"Update product with id {updatedProduct.Id}");
            return updatedProduct;
            
        }
        
        catch (Exception e)
        {
            logger.LogInformation($"Error updating product: {e.Message}");
            throw;
        }
    }

    public async Task<Product> Delete(ProductId productId, CancellationToken cancellationToken)
    {
        try
        {
            var product = await productRepository.GetById(productId, cancellationToken);
            if (product is null)
            {
                logger.LogInformation($"Product with id {productId} not found");
                throw new ArgumentException("Product not found");
            }
            await productRepository.Delete(product, cancellationToken);
            logger.LogInformation($"Delete product with id {product.Id}");
            return product;
        }
        catch (Exception e)
        {
            logger.LogInformation($"Error deleting product: {e.Message}");
            throw;
        }
    }
}
