using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Tbsi.Workshop.EfCore.Demo.Shared;

/*
 * Fix these poorly designed classes in Workshop step 3.
 * 
 * We want to eliminate infrastructure concerns from our domain models, so that they're largely
 * persistence ignorant.
 */

[Table("Authors")]
public class Author
{
    [Key]
    public Guid Id { get; set; }
    
    [Required, MaxLength(100)]
    public string Name { get; set; }
    
    public List<string> PenNames { get; set; }

    public List<Book> Books { get; set; }
}

[Table("Books")]
public class Book
{
    [Key]
    public Guid Id { get; set; }

    [Required, MaxLength(200)]
    public string Title { get; set; }
    
    [Column(TypeName = "text")]
    public BookCategory BookCategory { get; set; }

    [ForeignKey(nameof(AuthorId))]
    public Author Author { get; set; }
    
    [Required]
    public Guid AuthorId { get; set; }
}

public enum BookCategory
{
    Fiction = 0,
    Horror = 1,
}