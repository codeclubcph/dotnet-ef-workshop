using Testcontainers.PostgreSql;

namespace Tbsi.Workshop.EfCore.Demo.Shared;

/*
 * Create the database fixture that starts a postgres:16.2 testcontainer.
 */
public class DatabaseFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer container = new PostgreSqlBuilder()
        .WithImage("postgres:16.2")
        .Build();
    
    public string ConnectionString => container.GetConnectionString();
    
    public Task InitializeAsync()
    {
        return container.StartAsync();
    }

    public Task DisposeAsync()
    {
        return container.StopAsync();
    }
}