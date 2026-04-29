using ComputerStore.Data.Models;
using Microsoft.EntityFrameworkCore;
using static ComputerStore.Common.ApplicationConstants;

namespace ComputerStore.Data;

public class ComputerStoreDbContext : DbContext
{
    public virtual DbSet<User> Users { get; set; } = null!;
    public virtual DbSet<Category> Categories { get; set; } = null!;
    public virtual DbSet<Manufacturer> Manufacturers { get; set; } = null!;
    public virtual DbSet<PcPart> PcParts { get; set; } = null!;
    public virtual DbSet<Order> Orders { get; set; } = null!;
    public virtual DbSet<OrderItem> OrderItems { get; set; } = null!;

    public ComputerStoreDbContext(DbContextOptions<ComputerStoreDbContext> options)
        : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseSqlite(ConnectionString);

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .Property(u => u.Role)
            .HasConversion<string>();

        modelBuilder.Entity<Order>()
            .Property(o => o.Status)
            .HasConversion<string>();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username).IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email).IsUnique();

        modelBuilder.Entity<Manufacturer>()
            .HasIndex(m => m.Name).IsUnique();
    }
}
