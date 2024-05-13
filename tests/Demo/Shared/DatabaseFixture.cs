using Testcontainers.PostgreSql;

namespace Tbsi.Workshop.EfCore.Demo.Shared;

/*
 * Create the database fixture that starts a postgres:16.2 testcontainer.
 */
public class DatabaseFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer container = null!; // <- implement
    
    public string ConnectionString => container.GetConnectionString();
    
    public Task InitializeAsync()
    {
        throw new NotImplementedException();
    }

    public Task DisposeAsync()
    {
        throw new NotImplementedException();
    }
}