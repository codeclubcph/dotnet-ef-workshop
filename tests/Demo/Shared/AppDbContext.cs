using Microsoft.EntityFrameworkCore;

namespace Tbsi.Workshop.EfCore.Demo.Shared;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public const string DefaultSchema = "demo";

    // Add DbSet Author and Book properties 
    
    // Add DbSet Employees 

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(DefaultSchema);
        
        // Apply configuration classes (in step 3 - persistence ignorance)
    }
}