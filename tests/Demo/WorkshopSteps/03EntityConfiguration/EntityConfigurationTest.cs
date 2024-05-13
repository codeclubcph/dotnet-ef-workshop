using System.Text;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Tbsi.Workshop.EfCore.Demo.Shared;
using Xunit.Abstractions;

namespace Tbsi.Workshop.EfCore.Demo.WorkshopSteps._03EntityConfiguration;

// Open this step's readme.md before moving on.

[Collection("database")]
public class EntityConfigurationTest(
        ITestOutputHelper outputHelper,
        DatabaseFixture dbFixture,
        DbContextFixture contextFixture) 
    : IClassFixture<DatabaseFixture>, IClassFixture<DbContextFixture>
{
    
    [Fact]
    public void SaveAuthorWithBook()
    {
        using AppDbContext context = contextFixture.GetContext(dbFixture.ConnectionString, outputHelper);
        context.Database.BeginTransaction();
        
        // Remove the "NotImplementedException" then
        // create and save an author with two books.
        throw new NotImplementedException();
        
        
        context.Database.RollbackTransaction();
    }

    /// <summary>
    /// Try to fetch all authors
    /// </summary>
    [Fact]
    public void FetchAllAuthorsWithBooks()
    {
        throw new NotImplementedException();
    }
}