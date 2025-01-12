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
    public AppDbContextDesignTimeFactory() { }

    public AppDbContextDesignTimeFactory(string connectionString, Action<string> logSink = null!)
    {
        this.connectionString = connectionString;
        this.logSink = logSink;
    }
    
    private readonly string connectionString = DbConstants.PostgresConnectionString;
    private readonly Action<string> logSink = Console.WriteLine;
    
    public AppDbContext CreateDbContext(string[] args)
    {
        NpgsqlDataSource dataSource = new NpgsqlDataSourceBuilder(connectionString)
            .EnableDynamicJson()
            .Build();
        
        var builder = new DbContextOptionsBuilder<AppDbContext>()
                .UseNpgsql(dataSource, options =>
                {
                    options.MigrationsHistoryTable("__EFMigrationsHistory", AppDbContext.DefaultSchema);
                })
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging()
                .LogTo(logSink, LogLevel.Information)
            ;
        
        return new AppDbContext(builder.Options);
    }
}