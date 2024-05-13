using Tbsi.Workshop.EfCore.Demo.Shared;
using Xunit.Abstractions;

namespace Tbsi.Workshop.EfCore.Demo.WorkshopSteps._04Relations;

public class RelationTest(
    ITestOutputHelper outputHelper,
    DatabaseFixture database,
    DbContextFixture contextFixture) : IClassFixture<DatabaseFixture>
{
    [Fact]
    public void SaveAnAuthorWithContactDetails()
    {
        using AppDbContext context = contextFixture.GetContext(database.ConnectionString, outputHelper);

        throw new NotImplementedException();
    }

    [Fact]
    public void SaveBookWithTwoAuthors()
    {
        throw new NotImplementedException();
    }
}