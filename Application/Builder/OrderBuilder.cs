using Application.Common.Interfaces.Builders;
using Domain.Orders;
using Domain.Products;

namespace Application.Builder;

public class OrderBuilder: IOrderBuilder
{
    private Order _order;

    public OrderBuilder()
    {
        _order = Order.New(OrderId.New());
    }

    public IOrderBuilder AddProduct(Product product)
    {
        _order.AddProduct(product);
        return this;
    }

    public IOrderBuilder CalculateTotal()
    {
        _order.CalculateTotal();
        return this;
    }

    public Order Build()
    {
        var order = _order;
        _order = Order.New(OrderId.New());
        return order;
    }
}