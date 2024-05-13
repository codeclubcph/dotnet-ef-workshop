using System.Linq.Expressions;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Tbsi.Workshop.EfCore.Demo.Shared;


// Implement the IEntityTypeConfiguration<Author> interface
// The author should have the following configuration:
//     Table name -> "Authors"
//     Key -> Id property 
//     Name -> max length: 100, required, unique
//     PenNames -> required
//     Books -> many-to-one, foreign key: "AuthorId", on delete: restrict
public class AuthorsConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        
    }
}

// Implement the IEntityTypeConfiguration<Book> interface
// The book should have the following configuration:
//     Table name -> "Books"
//     Key -> Id property
//     Title -> max length: 200, required, index
//     Author -> one-to-many, foreign key: "AuthorId", required
//     BookCategory -> conversion to string, check constraint: only allow valid enum values
public class BooksConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        
    }
}