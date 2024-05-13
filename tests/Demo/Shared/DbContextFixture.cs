using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace Tbsi.Workshop.EfCore.Demo.Shared;

public class DbContextFixture
{
    public AppDbContext GetContext(string connectionString, ITestOutputHelper output)
    {
        AppDbContext appDbContext = new AppDbContextDesignTimeFactory(/*add connectionstring and output*/)
            .CreateDbContext(null!);

        appDbContext.Database.Migrate();
        
        return appDbContext;
    }
}