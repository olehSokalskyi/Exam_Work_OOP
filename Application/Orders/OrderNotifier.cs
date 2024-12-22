using System.Collections;
using Application.Common.Interfaces;
using Domain.Orders;

namespace Application.Orders;

public class OrderNotifier : IEnumerable<IObserver>, IOrderNotifier
{
    private readonly List<IObserver> _observers = new List<IObserver>();

    public void Attach(IObserver observer)
    {
        _observers.Add(observer);
    }

    public void Detach(IObserver observer)
    {
        _observers.Remove(observer);
    }

    public void NotifyUpdate(Order order)
    {
        foreach (var observer in _observers)
        {
            observer.Update(order);
        }
    }

    public void NotifyAdd(Order order)
    {
        foreach (var observer in _observers)
        {
            observer.Add(order);
        }
    }

    public void NotifyDelete(Order order)
    {
        foreach (var observer in _observers)
        {
            observer.Delete(order);
        }
    }

    public IEnumerator<IObserver> GetEnumerator()
    {
        return _observers.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}