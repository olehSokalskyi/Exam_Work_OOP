using Domain.Orders;
using Domain.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Implementation.Persistance.Configurations;

public class OrderConfiguration: IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasConversion(x => x.Value, x => new OrderId(x));
        
        builder.HasKey(o => o.Id);

        builder.HasMany(x => x.Products)
            .WithMany(x => x.Orders)
            .UsingEntity<Dictionary<string, object>>(
                "OrderProduct",
                j => j.HasOne<Product>().WithMany().HasForeignKey("ProductId"),
                j => j.HasOne<Order>().WithMany().HasForeignKey("OrderId"));
        
        builder.Property(x => x.TotalAmount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");
    }
}