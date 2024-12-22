using Application.Common.Interfaces;
using Application.Common.Interfaces.Builders;
using Application.Common.Interfaces.Repositories;
using Domain.Orders;
using Domain.Products;


namespace Application.Orders;

public class OrderService(IOrderRepository orderRepository, ILogger logger, IOrderBuilder orderBuilder,
    IProductRepository productRepository, IOrderNotifier orderNotifier) : IOrderService
{
    public async  Task<IReadOnlyList<Order>> GetAll(CancellationToken cancellationToken)
    {
        try
        {
            var orders = await orderRepository.GetAll(cancellationToken);
            logger.LogInformation("GetAll orders");
            return orders;    
        }
        catch (Exception e)
        {
            logger.LogInformation($"Error getting orders: {e.Message}");
            throw;
        }
        
    }

    public async Task<Order> Add(IList<ProductId> products, CancellationToken cancellationToken)
    {
        try
        {
            foreach(var productId in products)
            {
                var product = await productRepository.GetById(productId, cancellationToken);
                if(product is null)
                {
                    logger.LogInformation($"Product with id {product} not found");
                    throw new Exception("Product not found");
                }
                orderBuilder.AddProduct(product);
            }
            var order = orderBuilder.CalculateTotal().Build();
            await orderRepository.Add(order, cancellationToken);
            orderNotifier.NotifyAdd(order);
            logger.LogInformation($"Add order with id {order.Id}");
            return order;
        }
        catch (Exception e)
        {
            logger.LogInformation($"Error adding order: {e.Message}");
            throw;
        }
    }

    public async Task<Order> GetById(OrderId id, CancellationToken cancellationToken)
    {
        try{
            var order = await orderRepository.GetById(id, cancellationToken);
            if(order is null)
            {
                logger.LogInformation($"Order with id {id} not found");
                throw new Exception("Order not found");
            }
            logger.LogInformation($"Get order with id {id}");
            return order;
        }
        catch (Exception e)
        {
            logger.LogInformation($"Error getting order: {e.Message}");
            throw;
        }
    }

    public async Task<Order> Update(OrderId orderId, IList<ProductId> products, CancellationToken cancellationToken)
    {
        try
        {
            var order = await orderRepository.GetById(orderId, cancellationToken);
            if(order is null)
            {
                logger.LogInformation($"Order with id {orderId} not found");
                throw new Exception("Order not found");
            }
            order.ClearProducts();
            foreach(var productId in products)
            {
                
                var product = await productRepository.GetById(productId, cancellationToken);
                if(product is null)
                {
                    logger.LogInformation($"Product with id {product} not found");
                    throw new Exception("Product not found");
                }
                order.AddProduct(product);
            }
            order.CalculateTotal();
            await orderRepository.Update(order, cancellationToken);
            orderNotifier.NotifyUpdate(order);
            logger.LogInformation($"Update order with id {order.Id}");
            return order;
        }
        catch (Exception e)
        {
            logger.LogInformation($"Error updating order: {e.Message}");
            throw;
        }
    }

    public async Task<Order> Delete(OrderId orderId, CancellationToken cancellationToken)
    {
        try
        {
            var order = await orderRepository.GetById(orderId, cancellationToken);
            if(order is null)
            {
                logger.LogInformation($"Order with id {orderId} not found");
                throw new Exception("Order not found");
            }
            await orderRepository.Delete(order, cancellationToken);
            orderNotifier.NotifyDelete(order);
            logger.LogInformation($"Delete order with id {order.Id}");
            return order;
        }
        catch (Exception e)
        {
            logger.LogInformation($"Error deleting order: {e.Message}");
            throw;
        }
    }
    
    
}