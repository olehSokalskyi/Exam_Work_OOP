using Implementation.Persistance;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Tests.Common;

public abstract class BaseTest : IClassFixture<TestFactory>
{
    protected readonly ApplicationDbContext Context;

    protected BaseTest(TestFactory factory)
    {
        var scope = factory.ServiceProvider.CreateScope();

        Context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
    }

    protected async Task<int> SaveChangesAsync()
    {
        var result = await Context.SaveChangesAsync();

        Context.ChangeTracker.Clear();

        return result;
    }
}