using Domain.Products;

namespace Domain.Orders;

public class Order
{
    public OrderId Id { get; }
    public IList<Product> Products { get; private set; }
    public decimal TotalAmount { get; private set; }

    public Order(OrderId id)
    {
        Id = id;
        Products = new List<Product>();
        TotalAmount = 0;
    }

    public static Order New(OrderId id)
        => new(id);
    
    public void ClearProducts() =>
        Products.Clear();
    public void AddProduct(Product product) =>
        Products.Add(product);
    

    public void CalculateTotal() =>
        TotalAmount = Products.Sum(p => p.Price);
    
       
}