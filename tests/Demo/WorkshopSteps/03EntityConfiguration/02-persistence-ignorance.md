# Persistence Ignorance and Entity Configuration
> 10 minutes

Using data annotations tightly couples our domain models to persistence concerns. We want to avoid this since domain models
should ideally be persistence ignorant, meaning, they should not care about how they're stored and reconstituted.

> Do not compromise your domain model to cater for an external framework.

## Task 1: Remove persistence concerns

Adhere to the Persistence Ignorance principle by removing infrastructure concerns from the models.
1. Remove attributes from the `Author` and `Book` classes.
2. Remove navigation properties such as `Book.AuthorId` and the `[ForeignKey(nameof(AuthorId))]`

<details>
<summary><b>Hint: Removing persistence concerns</b></summary>

```csharp
public class Author
{
    private readonly List<Book> books = [];

    public Author(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
    }
    
    public Guid Id { get; init; }
    
    public string Name { get; private set; }

    public List<string> PenNames { get; } = [];

    public IReadOnlyList<Book> Books => books.AsReadOnly();

    public Book NewBook(string title, BookCategory category)
    {
        var book = new Book(title, category, this);
        books.Add(book);
        
        return book;
    }
}

public class Book
{
    private Book()
    {
        Id = Guid.NewGuid();
    }
    
    public Book(string title, BookCategory bookCategory, Author author) : this()
    {
        Title = title;
        BookCategory = bookCategory;
        Author = author;
    }
    
    public Guid Id { get; }

    public string Title { get; private set; }
    
    public BookCategory BookCategory { get; private set; }

    public Author Author { get; private set; }
}
```
</details>

Now, having removed the data annotations, the models look better but EF core may have a harder time understanding our intent.

For instance, removing `[Required, MaxLength(100)]` from `Author.Name` means EF Core thinks any text length is allowed.


## Task 2: Add entity configuration programmatically

We need to add "fluent configuration" to EF Core by using the `IEntityTypeConfiguration<T>` interface.

1. Delete the `Migrations` folder.
2. Open the [Shared/ModelEntityConfigurations.cs](../../Shared/ModelEntityConfigurations.cs) file and implement the `IEntityTypeConfiguration<T>` for
   each domain model.
3. Run the CLI command `dotnet-ef migration add Initial` again and inspect the output in the `Migrations` folder.

<details>
<summary><b>Hint: Model configurations</b></summary>

```csharp
public class AuthorsConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.ToTable("Authors");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Name).HasMaxLength(100).IsRequired();
        builder.Property(a => a.PenNames).IsRequired();

        builder.HasIndex(a => a.Name).IsUnique();

        builder.HasMany(a => a.Books)
            .WithOne(b => b.Author)
            .HasForeignKey("AuthorId")
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class BooksConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable("Books", table =>
        {
            var allowedValues = $"{string.Join(", ", Enum.GetValues<BookCategory>())}";
            table.HasCheckConstraint("chk_books_bookcategory", $" \"{nameof(Book.BookCategory)}\" = ANY ('{{ {allowedValues} }}'::TEXT[])");
        });
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Title).HasMaxLength(200).IsRequired();
        builder.Property(b => b.BookCategory).HasConversion<string>();

        builder.HasIndex(b => b.Title);
        
        builder.HasOne(b => b.Author)
            .WithMany(a => a.Books)
            .HasForeignKey("AuthorId");
    }
}
```
</details>

The two configuration classes won't take effect just yet. You need to tell the `DbContext` to look for 
configuration classes within an assembly.

Open the [AppDbContext.cs](../../Shared/AppDbContext.cs) and add `modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);`.


## Task 4: Generate a migration

We're now ready once again to generate a migration using the `dotnet-ef` tool.

> From the `tests/Demo` folder run `dotnet ef migrations add Initial`

## Task 5: Save and fetch entities

Go to the test class [EntityConfigurationTest.cs](EntityConfigurationTest.cs) and try to:
1. insert an author with a book,
2. and fetch all authors.

### [Go to next task](03-scripting-migrations.md) 