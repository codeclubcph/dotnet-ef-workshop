using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Tbsi.Workshop.EfCore.Demo.Shared;

/*
 * Fix these poorly designed classes in Workshop step 3.
 * 
 * We want to eliminate infrastructure concerns from our domain models, so that they're largely
 * persistence ignorant.
 */

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

    public IReadOnlyList<Book> Books => books;

    public ContactDetails? ContactDetails { get; set; }

    public Book NewBook(string title, BookCategory category)
    {
        var book = new Book(title, category, [this]);
        books.Add(book);
        
        return book;
    }
}

public record ContactDetails(string Email);


public class Book
{
    private Book()
    {
        Id = Guid.NewGuid();
    }
    
    public Book(string title, BookCategory category, List<Author> authors) : this()
    {
        Title = title;
        BookCategory = category;
        Authors = authors;
    }
    
    public Guid Id { get; set; }

    public string Title { get; set; }
    
    public BookCategory BookCategory { get; set; }

    public List<Author> Authors { get; } = [];

    public List<IReview> Reviews { get; } = [];
}

[JsonPolymorphic(TypeDiscriminatorPropertyName = "@type")]
[JsonDerivedType(typeof(AnonymousReview), nameof(AnonymousReview))]
[JsonDerivedType(typeof(UserReview), nameof(UserReview))]
[JsonDerivedType(typeof(VerifiedBuyerReview), nameof(VerifiedBuyerReview))]
public interface IReview
{
    string Text { get; }
    int Rating { get; }
}

public record AnonymousReview(string Text, int Rating) : IReview;
public record UserReview(string Text, int Rating, string Username) : IReview;
public record VerifiedBuyerReview(string Text, int Rating, string Username, Guid PurchaseId) : IReview;


public enum BookCategory
{
    Fiction = 0,
    Horror = 1,
    Romance = 3,
}


public abstract class Employee
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}

public class Editor : Employee
{
    public List<Book> Books { get; set; } = [];
}


public class Designer : Employee
{
    public SeniorityLevel Level { get; set; } = SeniorityLevel.Junior;
}

public enum SeniorityLevel
{
    Junior,
    Senior
}

