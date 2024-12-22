using Application.Common.Interfaces;
using Domain.Orders;

namespace Application.Orders;

public interface IOrderNotifier
{
    void Attach(IObserver observer);
    void Detach(IObserver observer);
    void NotifyUpdate(Order order);
    void NotifyAdd(Order order);
    void NotifyDelete(Order order);
    IEnumerator<IObserver> GetEnumerator();
}