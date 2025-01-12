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
        builder.ToTable("Authors");
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Name).IsRequired().HasMaxLength(100);
        builder.HasIndex(a => a.Name).IsUnique();
        builder.Property(a => a.PenNames).IsRequired();

        builder.Property<long>("ObjectId").IsRequired().UseIdentityColumn();
        builder.HasIndex("ObjectId").IsUnique();

        builder.OwnsOne(a => a.ContactDetails, cdBuilder =>
        {
            cdBuilder.Property(cd => cd.Email)
                .HasColumnName(nameof(ContactDetails.Email))
                .IsRequired(false);
        });

        builder.HasMany(a => a.Books)
            .WithMany(b => b.Authors)
            .UsingEntity("AuthorBook", ab =>
            {
                ab.HasOne(typeof(Author)).WithMany().HasForeignKey("AuthorsId").HasPrincipalKey("ObjectId");
            });
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
        builder.ToTable("Books", table =>
        {
            string allowedValues = string.Join(",", Enum.GetValues<BookCategory>());
            table.HasCheckConstraint("chk_Books_BookCategory", 
                $"\"BookCategory\" = ANY ( '{{ {allowedValues} }}'::TEXT[] )");
        });
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Title).IsRequired().HasMaxLength(200);
        builder.Property(b => b.BookCategory)
            .HasMaxLength(50)
            .HasConversion<string>();

        builder.Property(b => b.Reviews).HasColumnType("json");
    }
}

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employees");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Name).HasMaxLength(50).IsRequired();

        builder.HasDiscriminator<string>("EmployeeType")
            .HasValue<Designer>(nameof(Designer))
            .HasValue<Editor>(nameof(Editor));
    }
}

public class DesignerConfiguration : IEntityTypeConfiguration<Designer>
{
    public void Configure(EntityTypeBuilder<Designer> builder)
    {
        builder.Property(d => d.Level).HasConversion<string>();
    }
}

