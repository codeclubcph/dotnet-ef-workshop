using Microsoft.EntityFrameworkCore;
using Tbsi.Workshop.EfCore.Demo.Shared;
using Xunit.Abstractions;

namespace Tbsi.Workshop.EfCore.Demo.WorkshopSteps._06Json;

public class JsonTest(ITestOutputHelper outputHelper, DatabaseFixture database, DbContextFixture contextFixture) 
    : IClassFixture<DatabaseFixture>, IClassFixture<DbContextFixture>
{
    [Fact]
    public void SaveBookWithReviews()
    {
        using AppDbContext context = contextFixture.GetContext(DbConstants.PostgresConnectionString, outputHelper);
        
        // Arrange
        // Create an author and new book 

        var author = new Author("Nick");
        Book book = author.NewBook("Hello", BookCategory.Fiction);
        book.Reviews.Add(new AnonymousReview("whafa", 1));
        book.Reviews.Add(new UserReview("whafa", 1, "Nick"));
        book.Reviews.Add(new VerifiedBuyerReview("whafa", 1, "Nick", Guid.NewGuid()));

        context.Authors.Add(author);
        context.SaveChanges();

        // Save author with books
    }
}