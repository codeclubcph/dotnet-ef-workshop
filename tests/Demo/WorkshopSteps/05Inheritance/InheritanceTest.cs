using Microsoft.EntityFrameworkCore;
using Tbsi.Workshop.EfCore.Demo.Shared;
using Xunit.Abstractions;

namespace Tbsi.Workshop.EfCore.Demo.WorkshopSteps._05Inheritance;

public class InheritanceTest(ITestOutputHelper outputHelper, DatabaseFixture database, DbContextFixture dbFixture) 
    : IClassFixture<DatabaseFixture>, IClassFixture<DbContextFixture>
{
    [Fact]
    public void SaveEmployeesWithBooks()
    {
        using AppDbContext context = dbFixture.GetContext(DbConstants.PostgresConnectionString, outputHelper);

        context.Employees.Add(new Editor
        {
            Name = "Nick"
        });
        context.Employees.Add(new Designer
        {
            Name = "Nick2",
            Level = SeniorityLevel.Junior
        });

        context.SaveChanges();
    }

    [Fact]
    public void FetchEditorObjectGraph()
    {
        using AppDbContext context = dbFixture.GetContext(DbConstants.PostgresConnectionString, outputHelper);
        List<Employee> emps = context.Employees
            .TagWith("Get all employees")
            .Include(e => ((Editor)e).Books)
            .ThenInclude(b => b.Authors)
            .AsSplitQuery()
            .ToList();
    }

    [Fact]
    public void FetchOnlyEditors()
    {
        using AppDbContext context = dbFixture.GetContext(database.ConnectionString, outputHelper);
        
        List<Editor> emps = context.Employees
            .TagWith("Get all employees")
            .OfType<Editor>()
            .Include(e => e.Books)
            .ThenInclude(b => b.Authors)
            .AsSplitQuery()
            .ToList();
    }
}