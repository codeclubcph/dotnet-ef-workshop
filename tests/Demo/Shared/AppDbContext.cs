using Microsoft.EntityFrameworkCore;

namespace Tbsi.Workshop.EfCore.Demo.Shared;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public const string DefaultSchema = "demo";

    // Add DbSet Author and Book properties 
    public DbSet<Author> Authors { get; set; }
    public DbSet<Book> Books { get; set; }
    
    // Add DbSet Employees 
    public DbSet<Employee> Employees { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(DefaultSchema);
        
        // Apply configuration classes (in step 3 - persistence ignorance)
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}