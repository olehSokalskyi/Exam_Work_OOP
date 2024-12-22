using Microsoft.EntityFrameworkCore;

namespace Implementation.Persistance;

public class ApplicationDbContextInitialiser(ApplicationDbContext context)
{
    public async Task InitializeAsync()
    {
        await context.Database.MigrateAsync();
    }
}