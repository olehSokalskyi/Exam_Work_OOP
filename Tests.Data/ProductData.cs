using Domain.Products;

namespace Tests.Data;

public class ProductData
{
    public static Product ProductOne => Product.New(new ProductId(Guid.Parse("1ac9ff90-2d7e-43cc-bb81-28045c67cf56")), "Product One", 10.00m);
    public static  Product  ProductTwo => Product.New(ProductId.New(),"Product Two", 20.00m);
}