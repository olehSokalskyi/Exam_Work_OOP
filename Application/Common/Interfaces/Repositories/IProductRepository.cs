using Domain.Products;

namespace Application.Common.Interfaces.Repositories;

public interface IProductRepository
{
    public Task<Product> Add(Product product, CancellationToken cancellationToken);
    public Task<Product> GetById(ProductId id, CancellationToken cancellationToken);
    public Task<Product> Update(Product product, CancellationToken cancellationToken);
    public Task<Product> Delete(Product product, CancellationToken cancellationToken);
    public Task<Product> GetByName(string name, CancellationToken cancellationToken);
    Task<IReadOnlyList<Product>> GetAll(CancellationToken cancellationToken);
}