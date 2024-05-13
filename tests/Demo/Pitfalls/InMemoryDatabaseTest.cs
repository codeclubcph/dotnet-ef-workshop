using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Tbsi.Workshop.EfCore.Demo.Shared;

namespace Tbsi.Workshop.EfCore.Demo.Pitfalls;


public class InMemoryDatabaseTest
{
    public InMemoryDatabaseTest()
    {
        Context = new AppDbContextDesignTimeFactory().CreateDbContext(null!);
    }

    public AppDbContext Context { get; set; }
    
    /*
     *
     * CREATE TABLE "Authors" (
     *  "Id" uuid NOT NULL,
     *  "Name" text NOT NULL,
     *  -- and many other columns
     * CONSTRAINT "PK_Users" PRIMARY KEY ("Id")
     * ); 
     */
    [Fact]
    public void SavingInvalidData()
    {
        var context = new InMemoryDbContext();
        
        var author = new BadAuthor
        {
            Name = "Nick",
            Books = [new BadBook
            {
                Title = "Hello there"
            }]
        };

        context.Authors.Add(author);
        context.SaveChanges();
    }
}


public class InMemoryDbContext : DbContext
{
    public DbSet<BadAuthor> Authors { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
    }
}

public class BadAuthor
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<BadBook> Books { get; init; } = [];
}

public class BadBook
{
    public Guid Id { get; set; }
    public string Title { get; set; }
}

public interface IReview
{
    Guid Id { get; }
    string Text { get; }
}

public class SimpleReview : IReview
{
    public Guid Id { get; init; }
    public string Text { get; init; }
}

public class RatingReview : IReview
{
    public Guid Id { get; init; }
    public string Text { get; init; }
    public int Rating { get; set; }
}


