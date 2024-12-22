using Domain.Products;

namespace Application.Common.Interfaces.Services;

public interface IProductService
{
    public Task<IReadOnlyList<Product>> GetAll(CancellationToken cancellationToken);
    Task<Product> Add(string name, decimal price, CancellationToken cancellationToken);
    public Task<Product> GetById(ProductId id, CancellationToken cancellationToken);
    Task<Product> Delete(ProductId productId, CancellationToken cancellationToken);
    Task<Product> Update(ProductId productId, string name, decimal price, CancellationToken cancellationToken);
}