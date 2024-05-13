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
        
        // Add reviews
        throw new NotImplementedException("Add reviews to the book");
        
        // Save author with books
    }
}