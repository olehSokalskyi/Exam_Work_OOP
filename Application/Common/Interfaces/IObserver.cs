using Domain.Orders;

namespace Application.Common.Interfaces;

public interface IObserver
{
    void Update(Order order);
    void Add(Order order);
    void Delete(Order order);
}