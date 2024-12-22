using Domain.Orders;

namespace Domain.Products;

public class Product
{
    public ProductId Id { get; }
    public string Name { get; private set; }
    public decimal Price { get; private set; }
    public IList<Order> Orders { get; private set; }
    private Product(ProductId id, string name, decimal price)
    {
        Id = id;
        Name = name;
        Price = price;
        Orders = new List<Order>();
    }
    
    public static Product New(ProductId id, string name, decimal price)
        => new(id, name, price);
    
    public void UpdateDetails(string name, decimal price)
    {
        Name = name;
        Price = price;
    }
    
}