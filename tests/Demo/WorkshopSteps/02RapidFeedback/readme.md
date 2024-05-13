# Rapid feedback
> 5 minutes

We want to get into the habit of getting quick, reliable feedback as soon as possible—one way to do just that 
is by writing simple experimentation tests.

In this step, we'll create a test fixture which allows us to start a real postgres server whenever we run a test.

When testing your database interactions, you'll want to run queries against a real database and **not** an in-memory 
database one, since only real queries provide an accurate reflection of how your system behaves.

## Task 1 – Test Fixture

Create a Test Fixture class that starts a postgres container (image: postgres:16.2).

1. Implement the rest of the class in the [Shared/DatabaseFixture.cs](../../Shared/DatabaseFixture.cs) file.

<details>
<summary><b>Hint: Database fixture implementation</b></summary>

```csharp
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
```
</details>

## Task 2 – Run a simple query
Go to the [TestcontainersTest.cs](TestcontainersTest.cs) file. Then:
1. Use the class fixture
2. Connect to the database and run a simple query like `SELECT timezone('utc', now())`.

<details>
<summary><b>Hint: Simple SQL test query</b></summary>

```csharp
[Fact]
public async Task SimpleRawQuery()
{
    await using var conn = new NpgsqlConnection(fixture.ConnectionString);
    await conn.OpenAsync();

    NpgsqlCommand command = conn.CreateCommand();
    command.CommandText = "SELECT timezone('utc', now())";
    object? result = command.ExecuteScalar();

    outputHelper.WriteLine(result?.ToString());
}
```
</details>

### [Go to next step](../03EntityConfiguration/readme.md)