# Inheritance
> 10 minutes

While inheritance is prevalent in software development, it isn't really a concept used in SQL – this issue is also
a part of what's called the "object-relational impedance mismatch."

A common approach to inheritance is a "Table Per Hierarchy," where a base-class and its subclasses are all stored
in a single, shared table, using a "discriminator" column to decide which type to resolve at runtime. 


## Task 1: Add a hierarchy of classes

1. Open the [Shared/Models.cs](../../Shared/Models.cs) file and add the new classes `Employee`, `Designer`, `Editor`, and `EditorNote`.
2. Add a `DbSet<Employee>` property to the `AppDbContext`.

<details>
<summary><b>Hint: Add new class hierarchy</b></summary>

```csharp
// Models.cs
public abstract class Employee
{
    protected Employee(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
    }
    
    public Guid Id { get; init; }
    public string Name { get; private set; }
}

public class Designer(string name) : Employee(name)
{
    public DesignerLevel Level { get; set; }
    public List<Book> BookDesigns { get; } = [];
}

public enum DesignerLevel
{
    Junior,
    Senior
}

public class Editor(string name) : Employee(name)
{
    public List<EditorNote> EditorNotes { get; } = [];
}

public class EditorNote
{
    private EditorNote()
    {
        Id = Guid.NewGuid();
        Created = DateTimeOffset.UtcNow;   
    }
    
    public EditorNote(Book book, string note) : this()
    {
        Book = book;
        Note = note;
    }

    public Guid Id { get; init; }
    public Book Book { get; private set; }
    public string Note { get; private set; }
    public DateTimeOffset Created { get; set; }
}
```
</details>

<details>
<summary><b>Hint: Add new property to AppDbContext</b></summary>

```csharp
// AppDbContext.cs
public class AppDbContext : DbContext
{
    // New property
    public DbSet<Employee> Employees { get; set; }
}
```
</details>


## Task 2: Add entity configuration

After having added the classes, we'll need to configure them in order for EntityFramework to understand it should
use inheritance.

Inheritance configuration is slightly different, since you need to configure the base class and explicitly tell EF which
subtypes exist and their discriminators.

1. Open the [Shared/ModelEntityConfigurations.cs](../../Shared/ModelEntityConfigurations.cs) file
2. Add a new configuration for the `Employee` base class and configure its discriminator.
3. Add a separate configuration for the `Designer` subtype and let the enum `DesignerLevel` be converted to a string. 

<details>
<summary><b>Hint: Entity configuration</b></summary>

```csharp
// ModelEntityConfigurations.cs

// New class
public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employees");
        builder.HasKey(e => e.Id);
        
        builder.HasDiscriminator<string>("EmployeeType")
            .HasValue<Designer>(nameof(Designer))
            .HasValue<Editor>(nameof(Editor));
    }
}

// New class
public class DesignerConfiguration : IEntityTypeConfiguration<Designer>
{
    public void Configure(EntityTypeBuilder<Designer> builder)
    {
        builder.Property(d => d.Level).HasConversion<string>();

        builder.HasMany<Book>(d => d.BookDesigns)
            .WithOne();
    }
}

```
</details>

## Task 3: Querying inheritance

Querying the database for types of a hierarchy is also somewhat different, since the initial query is based off of the
base type.

Open the [InheritanceTest.cs](InheritanceTest.cs) file and implement the tests.