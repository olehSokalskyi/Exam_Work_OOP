using Application.Common.Interfaces.Repositories;
using Domain.Products;
using Microsoft.EntityFrameworkCore;

namespace Implementation.Persistance.Repositories;

public class ProductRepository(ApplicationDbContext context): IProductRepository
{
    public async Task<IReadOnlyList<Product>> GetAll(CancellationToken cancellationToken)
    {
        return await context.Products
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
    public async Task<Product> Add(Product product, CancellationToken cancellationToken)
    {
        var existingProduct = context.Products.Local.FirstOrDefault(x => x.Id == product.Id);
        if(existingProduct != null)
        {
            context.Entry(existingProduct).State = EntityState.Detached;
        }
        await context.Products.AddAsync(product, cancellationToken);
        
        await context.SaveChangesAsync(cancellationToken);
        context.ChangeTracker.Clear();
        return product;
    }

    public async Task<Product> GetById(ProductId id, CancellationToken cancellationToken)
    {
        var entity = await context.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity;
    }

    public async Task<Product> Update(Product product, CancellationToken cancellationToken)
    {
        var existingProduct = context.Products.Local.FirstOrDefault(x => x.Id == product.Id);
        if(existingProduct != null)
        {
            context.Entry(existingProduct).State = EntityState.Detached;
        }
        context.Products.Update(product);
        
        await context.SaveChangesAsync(cancellationToken);
        context.ChangeTracker.Clear();
        return product;
    }

    public async Task<Product> Delete(Product product, CancellationToken cancellationToken)
    {
        var existingProduct = context.Products.Local.FirstOrDefault(x => x.Id == product.Id);
        if(existingProduct != null)
        {
            context.Entry(existingProduct).State = EntityState.Detached;
        }
        context.Products.Remove(product);
        
        await context.SaveChangesAsync(cancellationToken);
        context.ChangeTracker.Clear();
        return product;
    }

    public async Task<Product> GetByName(string name, CancellationToken cancellationToken)
    {
        var entity = await context.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Name == name, cancellationToken);
        
        return entity;
    }
}