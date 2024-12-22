using Domain.Orders;

namespace Application.Common.Interfaces.Repositories;

public interface IOrderRepository
{
    public Task<Order> Add(Order order, CancellationToken cancellationToken);
    public Task<Order> GetById(OrderId id, CancellationToken cancellationToken);
    public Task<Order> Update(Order order, CancellationToken cancellationToken);
    public Task<Order> Delete(Order order, CancellationToken cancellationToken);
    Task<IReadOnlyList<Order>> GetAll(CancellationToken cancellationToken);
}