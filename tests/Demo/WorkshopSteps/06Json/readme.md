# Working with JSON
> 15 minutes

You'll often need to store complex structures that don't play nice with relational databases. This is especially true
if you want an interface property on your model and the database provider doesn't know how to map the property.

Let's see how we can use `ValueConverter<>` and Global configuration to store complex data as
JSON without compromising our domain model.


# Task 1: Add an interface and implementations

Say we want to add book reviews of different sorts to a book. We'll create an `IReview` interface and 
a few implementations to go with it.

```csharp
// Models.cs
public interface IReview
{
    string Text { get; }
    int Rating { get; }
}

public record AnonymousReview(string Text, int Rating) : IReview;
public record UserReview(string Text, int Rating, string Username) : IReview;
public record VerifiedBuyerReview(string Text, int Rating, string Username, Guid PurchaseId) : IReview;


public class Book {
    // New property
    public List<IReview> Reviews { get; } = [];    
}
```
Creating a migration at this point does nothing despite having added a property to the `Book` class.

## Task 2: Configure EF to see the reviews

1. Go to the [Shared/ModelEntityConfigurations.cs](../../Shared/ModelEntityConfigurations.cs) file. 
2. Configure the `Reviews` property in the `BooksConfiguration` class.

```csharp
public class BooksConfiguration : IEntityTypeConfiguration<Book> {
    
    // Add this
    builder.Property(b => b.Reviews)
        .HasColumnType("json");
}
```
Notice how we also explicitly tell EF to map this property to a `json` column. Omitting this would make EF throw an
error when generating a migration.

However, we're still not done just yet.

The database provider doesn't have a clue about what `List<IReview>` is and how to interpret that. So, we have two options:
- Allow dynamic JSON globally,
- or create a custom value converter that is invoked when saving and retrieving

Let's review both options.

### Allow JSON globally

```csharp
// AppDbContextDesignTimeFactory.cs
public class AppDbContextDesignTimeFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    // The data source can't use an empty string as the ConnectionString. So we'll
    // need to set it to something. In this case the DbConstants.PostgresConnectionString. 
    private readonly string connectionString = DbConstants.PostgresConnectionString;
    
    public AppDbContext CreateDbContext(string[] args)
    {
        // Use explicit data source
        NpgsqlDataSource source = new NpgsqlDataSourceBuilder(connectionString)
            .EnableDynamicJson() // <- this
            .Build();
        
        var builder = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(source,
                optionsBuilder =>
                {
                    optionsBuilder.MigrationsHistoryTable("__EFMigrationHistory", AppDbContext.DefaultSchema);
                })
            // other config omitted
            ;
        
        return new AppDbContext(builder.Options);
    }
}
```

### Custom value converter

Value converters are quite handy for taking charge of how things are serialized when saving and 
deserialized when fetching data.

```csharp
// ModelEntityConfigurations.cs

public class ReviewValueConverter() : ValueConverter<List<IReview>, string>(
    list => JsonSerializer.Serialize(list, Options), // <- C# to db value
    value => JsonSerializer.Deserialize<List<IReview>>(value, Options) ?? new List<IReview>()) // <- db value to C#
{
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
}

public class BooksConfiguration : IEntityTypeConfiguration<Book> 
{
    builder.Property(b => b.Reviews)
        .HasColumnType("json")
        .HasConversion<ReviewValueConverter>(); // <- Add this
}
```

## Task 3: Saving data

While attempting to save a book and a few reviews does work, we're still not seeing a satisfying result. 
Saving the book's reviews results in only the saving properties that belong to the `IReview` and not 
the individual subtypes. 

1. Open the [JsonTest.cs](JsonTest.cs) file
2. Add a book and a few reviews to it
3. Save the data and inspect the database table `demo.books`

```csharp
// Add reviews to book
book.Reviews.Add(new AnonymousReview("Great book!", 5));
book.Reviews.Add(new UserReview("Awesome book!", 4, "user123"));
book.Reviews.Add(new VerifiedBuyerReview("Excellent book!", 5, "user456", Guid.NewGuid()));
```

```json5
// result in database
[
  {
    "Text": "Great book!",
    "Rating": 5
  },
  {
    "Text": "Awesome book!",
    "Rating": 4
  },
  {
    "Text": "Excellent book!",
    "Rating": 5
  }
]
```

## Task 4: Saving concrete types

The issue with missing properties for concrete types is not an EF or database provider issue, it's just how C# generally
deals with serializing interfaces to JSON.  

We need to tell C# how we'd like our concrete classes to be serialized and deserialized.

1. Open the [Shared/Models.cs](../../Shared/Models.cs) file.
2. Add json attributes as shown below.

```csharp
// Models.cs
[JsonPolymorphic(TypeDiscriminatorPropertyName = "@type")]
[JsonDerivedType(typeof(AnonymousReview), nameof(AnonymousReview))]
[JsonDerivedType(typeof(UserReview), nameof(UserReview))]
[JsonDerivedType(typeof(VerifiedBuyerReview), nameof(VerifiedBuyerReview))]
public interface IReview
{
    string Text { get; }
    int Rating { get; }
}
```

1. Drop the database tables.
2. Without modifying the code in the [JsonTest.cs](JsonTest.cs) file, try to run the test again to save the book and its
   reviews.

Now, when saving our book's reviews, we'll see the expected result:

```json5
[
  {
    "@type": "AnonymousReview",
    "Text": "Great book!",
    "Rating": 5
  },
  {
    "@type": "UserReview",
    "Text": "Awesome book!",
    "Rating": 4,
    "Username": "user123"
  },
  {
    "@type": "VerifiedBuyerReview",
    "Text": "Excellent book!",
    "Rating": 5,
    "Username": "user456",
    "PurchaseId": "30674415-2e37-4023-a812-87e88d2b1be6"
  }
]
```
Notice the `@type` key. This is used by the serializer to figure out which conrete type to deserialize into.