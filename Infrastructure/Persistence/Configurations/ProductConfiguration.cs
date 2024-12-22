using Domain.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Implementation.Persistance.Configurations;

public class ProductConfiguration: IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasConversion(x => x.Value, x => new ProductId(x));
        
        builder.Property(x => x.Name)
            .IsRequired()
            .HasColumnType("varchar(255)");
        
        builder.Property(x => x.Price)
            .IsRequired()
            .HasColumnType("decimal(18,2)");
    }
}