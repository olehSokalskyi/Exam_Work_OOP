
using Domain.Orders;
using Domain.Products;

namespace Application.Common.Interfaces.Builders;

public interface IOrderBuilder
{
    public IOrderBuilder AddProduct(Product product);
    public IOrderBuilder CalculateTotal();
    public Order Build();
}