using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Tbsi.Workshop.EfCore.Demo.Shared;

// Implement the IDesignTimeDbContextFactory<AppDbContext> interface
//      We need two constructors - a default one used by EF Core
//      and a second one that takes a connection string and Action<string> for logging.
//      Also, enable sensitive data logging and detailed error logs.
public class AppDbContextDesignTimeFactory 
    : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<AppDbContext>()
            // Configure
            ;
        
        return new AppDbContext(builder.Options);
    }
}