using Microsoft.EntityFrameworkCore;
using Tbsi.Workshop.EfCore.Demo.Shared;
using Xunit.Abstractions;

namespace Tbsi.Workshop.EfCore.Demo.WorkshopSteps._05Inheritance;

public class InheritanceTest(ITestOutputHelper outputHelper, DbContextFixture database, DbContextFixture dbFixture) 
    : IClassFixture<DatabaseFixture>
{
    [Fact]
    public void SaveEmployeesWithBooks()
    {
        using AppDbContext context = dbFixture.GetContext(DbConstants.PostgresConnectionString, outputHelper);
        // Create author with book

        throw new NotImplementedException();
        // 1. Create a Designer and add the book to the "BookDesigns"
        
        // 2. Create an Editor and add an EditorNote for the Book
        
        context.SaveChanges();
    }

    [Fact]
    public void FetchEditorObjectGraph()
    {
        using AppDbContext context = dbFixture.GetContext(DbConstants.PostgresConnectionString, outputHelper);
        
        throw new NotImplementedException();
        // Fetch all employees and their object graph
    }

    [Fact]
    public void FetchOnlyEditors()
    {
        using AppDbContext context = dbFixture.GetContext(DbConstants.PostgresConnectionString, outputHelper);
        
        // Fetch just Editors and their object graph
        throw new NotImplementedException();
    }
}