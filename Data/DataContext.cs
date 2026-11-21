using Microsoft.EntityFrameworkCore;
using RestaurantApi.Entities;

namespace RestaurantApi.Data;

public sealed class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<Table> Tables => Set<Table>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(p => p.Price).HasPrecision(8, 2);
            entity.HasData(DataSeed.Products);
        });

        modelBuilder.Entity<Table>(entity =>
        {
            entity.HasIndex(t => t.Number).IsUnique();
            entity.HasData(DataSeed.Tables);
        });
    }
}