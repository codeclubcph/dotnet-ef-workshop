# Entity Framework Core Quick Recap

Entity Framework (EF) is the official data-access platform from Microsoft that helps developers write clean and portable applications
faster.

EF affords a range of capabilities within accessing and managing data, such as:
- Object-relation mapping (OR/M)
- Translating LINQ statements to SQL queries
- Schema migrations and tracking changes

EF has a wide range of database providers, as you can [see here](https://learn.microsoft.com/en-us/ef/core/providers/?tabs=dotnet-core-cli).

You might be familiar with other programming languages' popular OR/Ms such as [Eloquent for PHP](https://laravel.com/docs/11.x/eloquent),
[Hibernate for Java](https://hibernate.org/), [SQLAlchemy for Python](https://www.sqlalchemy.org/), and many others.

## Simple data access example

Our contrived domain model.
```c#
public class Author {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
}
```

Configure a `DbContext` to access the `Author` entities.
```c#
public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        Authors = Set<Author>();
    }

    public string Schema { get; set; } = "demo";

    public DbSet<Author> Authors { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);
    }
}
```

Running the `add-migration` CLI command will generate us a migration file that creates an `Authors` table with guid/uuid, nvarchar/text,
and int columns. The exact SQL will depend on the applied database provider.

`dotnet ef migrations add Initial`

Then use the `AppDbContext` in a class that needs to access `Author` entities.
```C#
public class GetAuthorsQuery {
    private readonly AppDbContext context;
    
    public GetAuthorsQuery(AppDbContext context) {
        this.context = context;
    }

    public IEnumerable<Author> ExecuteQuery() {
        return context.Authors.ToList();
    }
}
```
You see, working with Entity Framework Core is straightforward, quick, and quite painless.


> ! This example is not considered "production"-grade. We will learn more about how to produce production-ready code using Entity Framework 
Core in this workshop. 

[Go back to introduction](../readme.md)