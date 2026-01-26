using Microsoft.EntityFrameworkCore;
using Planara.Projects.Data.Domain;

namespace Planara.Projects.Data;

public class DataContext(DbContextOptions options): DbContext(options)
{
    public DbSet<Project> Projects { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Project>()
            .HasKey(x => x.Id);
    }
}