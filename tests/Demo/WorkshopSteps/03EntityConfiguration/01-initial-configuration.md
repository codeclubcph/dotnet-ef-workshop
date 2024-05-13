# 03 – Initial configuration
> 5 minutes

## Task 1 – Model check
Open the [Shared/Models.cs](../../Shared/Models.cs) and take a minute to inspect 
the `Author` and `Book`entity models. They're quite simple for now.

You don't need to do anything yet.

## Task 2 – AppDbContext
Open the [Shared/AppDbContext.cs](../../Shared/AppDbContext.cs) file.
1. Add a `DbSet<Author>` property.
2. Then add a `DbSet<Book>` property.

These two properties act as our access to each table allowing us to save, fetch, delete, and update data.

<details>
<summary><b>Hint: AppDbContext</b></summary>

```csharp
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public const string DefaultSchema = "demo";

    // Add DbSet Author and Book properties
    public DbSet<Author> Authors { get; set; }

    public DbSet<Book> Books { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(DefaultSchema);
        
        // Apply configuration classes (step 3 - persistence ignorance)
    }
}
```
</details>

## Task 3 – Try to create a migration
Based on our models that are configured using attribute (data annotations), create a migration using the dotnet-ef cli.

> From the `tests/Demo` folder run `dotnet ef migrations add Initial`.

You should now see an error telling us that dotnet-ef didn't know how to create the `DbContext`. To fix this, 
we need to implement a "design time factory."

Open the [Shared/AppDbContextDesignTimeFactory.cs](../../Shared/AppDbContextDesignTimeFactory.cs) file and implement the `IDesignTimeDbContextFactory<>` interface.

<details>
<summary><b>Hint: AppDbContextDesignTimeFactory implementation</b></summary>

```csharp
public class AppDbContextDesignTimeFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContextDesignTimeFactory() { }

    public AppDbContextDesignTimeFactory(string connectionString, Action<string>? logSink = null)
    {
        this.connectionString = connectionString;
        this.logSink = logSink ?? Console.WriteLine;
    }
    
    private readonly string connectionString = "";
    private readonly Action<string> logSink = Console.WriteLine;
    
    public AppDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(connectionString,
                optionsBuilder =>
                {
                    optionsBuilder.MigrationsHistoryTable("__EFMigrationHistory", AppDbContext.DefaultSchema);
                })
            .LogTo(logSink, [
                DbLoggerCategory.Query.Name,
                DbLoggerCategory.Migrations.Name,
                DbLoggerCategory.Database.Command.Name
            ], LogLevel.Information)
            .EnableDetailedErrors()
            .EnableSensitiveDataLogging();;
        
        return new AppDbContext(builder.Options);
    }
}
```
</details>

After setting up our design time factory, try to run the dotnet ef migrations add command again. You should now see a
new folder and file called `Migrations/<timestamp>_Initial.cs`. Take a minute to inspect the new file. Inspecting the 
generated files provides valuable insights into how EF Core has interpreted your models.

### [Go to next task](02-persistence-ignorance.md) 