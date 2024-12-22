using Domain.Orders;
using Domain.Products;

namespace Application.Orders;

public interface IOrderService
{
    public Task<IReadOnlyList<Order>> GetAll(CancellationToken cancellationToken);
    public Task<Order> Add(IList<ProductId> products, CancellationToken cancellationToken);
    public Task<Order> GetById(OrderId id, CancellationToken cancellationToken);
    public Task<Order> Delete(OrderId id, CancellationToken cancellationToken);
    Task<Order> Update(OrderId orderId, IList<ProductId> products, CancellationToken cancellationToken);
}