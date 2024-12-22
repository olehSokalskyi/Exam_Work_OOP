using Application.Common.Interfaces.Repositories;
using Domain.Orders;
using Microsoft.EntityFrameworkCore;

namespace Implementation.Persistance.Repositories;

public class OrderRepository(ApplicationDbContext context) : IOrderRepository
{
    public async Task<Order> Add(Order order, CancellationToken cancellationToken)
    {
        foreach (var product in order.Products)
        {
            var existingProduct = await context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == product.Id, cancellationToken);

            if (existingProduct == null)
            {
                await context.Products.AddAsync(product, cancellationToken);
            }
            else
            {
                context.Entry(product).State = EntityState.Modified;
            }
        }

        var existingOrder = context.Orders.Local.FirstOrDefault(x => x.Id == order.Id);
        if (existingOrder != null)
        {
            context.Entry(existingOrder).State = EntityState.Detached;
        }
        await context.Orders.AddAsync(order, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);
        context.ChangeTracker.Clear(); 
        return order;
    }

    public async Task<Order> GetById(OrderId id, CancellationToken cancellationToken)
    {
        var entity = await context.Orders
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity;
    }

    public async Task<Order> Update(Order order, CancellationToken cancellationToken)
    {
        foreach (var product in order.Products)
        {
            var existingProduct = await context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == product.Id, cancellationToken);

            if (existingProduct == null)
            {
                await context.Products.AddAsync(product, cancellationToken);
            }
            else
            {
                context.Entry(product).State = EntityState.Modified;
            }
        }

        var existingOrder = context.Orders.Local.FirstOrDefault(x => x.Id == order.Id);
        if (existingOrder != null)
        {
            context.Entry(existingOrder).State = EntityState.Detached;
        }
 
        context.Orders.Update(order);

        await context.SaveChangesAsync(cancellationToken);
        context.ChangeTracker.Clear();
        return order;
    }

    public async Task<Order> Delete(Order order, CancellationToken cancellationToken)
    {
        var existingOrder = context.Orders.Local.FirstOrDefault(x => x.Id == order.Id);
        if (existingOrder != null)
        {
            context.Entry(existingOrder).State = EntityState.Detached;
        }
        context.Orders.Remove(order);

        await context.SaveChangesAsync(cancellationToken);
        context.ChangeTracker.Clear();
        return order;
    }

    public async Task<IReadOnlyList<Order>> GetAll(CancellationToken cancellationToken)
    {
        return await context.Orders
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}