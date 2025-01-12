using Npgsql;
using Tbsi.Workshop.EfCore.Demo.Shared;
using Xunit.Abstractions;

namespace Tbsi.Workshop.EfCore.Demo.WorkshopSteps._02RapidFeedback;

// Open this step's readme.md to create a database fixture before moving on.

public class TestcontainersTest(ITestOutputHelper outputHelper, DatabaseFixture fixture) 
    : IClassFixture<DatabaseFixture> // <- Uncomment this
{
    /// <summary>
    /// Now use the fixture by accessing the postgres database using the exposed ConnectionString.
    /// 
    /// 1. Open a connection using npgsql
    /// 2. Execute a simple postgreSQL query ('SELECT timezone('utc', now())')
    /// </summary>
    [Fact]
    public async Task SimpleRawQuery()
    {
        await using var connection = new NpgsqlConnection(fixture.ConnectionString);
        await connection.OpenAsync();
        await using NpgsqlCommand command = connection.CreateCommand();
        command.CommandText = "SELECT timezone('utc', now())";

        object? result = command.ExecuteScalar();
        outputHelper.WriteLine(result?.ToString());
    }
}