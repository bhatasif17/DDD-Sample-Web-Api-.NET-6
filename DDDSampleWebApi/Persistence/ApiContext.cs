using DDDSampleWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DDDSampleWebApi.Persistence;

public class ApiContext : DbContext
{
    public ApiContext(DbContextOptions<ApiContext> options)
        : base(options)
    {
    }
    public DbSet<Hiker> Hikers { get; set; }
    public DbSet<ReportedHiker> ReportedHikers { get; set; }
    public DbSet<Items> Items { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Items>().HasData(
            new Items()
            {
                Id = 1,
                Name = "Water",
                Points = 4
            },
            new Items()
            {
                Id = 2,
                Name = "Food",
                Points = 3
            },
            new Items()
            {
                Id = 3,
                Name = "Medication",
                Points = 2
            },
            new Items()
            {
                Id = 4,
                Name = "Stick",
                Points = 1
            }
        );
    }
}

