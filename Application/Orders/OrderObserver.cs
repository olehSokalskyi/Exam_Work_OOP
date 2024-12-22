using Application.Common.Interfaces;
using Domain.Orders;

namespace Application.Orders;

public class OrderObserver(ILogger logger) : IObserver
{
    public void Update(Order order)
    {
        logger.LogInformation($"Order with ID {order.Id} has been updated. Total amount: {order.TotalAmount}");
    }

    public void Add(Order order)
    {
        logger.LogInformation($"Order with ID {order.Id} has been added. Total amount: {order.TotalAmount}");
    }

    public void Delete(Order order)
    {
        logger.LogInformation($"Order with ID {order.Id} has been deleted.");
    }
}