using ComputerStore.Data.Models;
using ComputerStore.Data.Models.Enums;
using Microsoft.EntityFrameworkCore;
using static ComputerStore.Common.ApplicationConstants;

namespace ComputerStore.Data
{
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

            // Seed data

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "CPU", Description = "Central Processing Units" },
                new Category { Id = 2, Name = "GPU", Description = "Graphics Processing Units" },
                new Category { Id = 3, Name = "RAM", Description = "Memory modules" },
                new Category { Id = 4, Name = "Storage", Description = "SSDs and HDDs" },
                new Category { Id = 5, Name = "Motherboard", Description = "Mainboards" },
                new Category { Id = 6, Name = "Power Supply", Description = "PSUs" },
                new Category { Id = 7, Name = "Case", Description = "PC Cases" },
                new Category { Id = 8, Name = "Cooling", Description = "CPU and case coolers" }
            );

            modelBuilder.Entity<Manufacturer>().HasData(
                new Manufacturer { Id = 1, Name = "Intel", Country = "USA", Website = "https://www.intel.com" },
                new Manufacturer { Id = 2, Name = "AMD", Country = "USA", Website = "https://www.amd.com" },
                new Manufacturer { Id = 3, Name = "NVIDIA", Country = "USA", Website = "https://www.nvidia.com" },
                new Manufacturer { Id = 4, Name = "Corsair", Country = "USA", Website = "https://www.corsair.com" },
                new Manufacturer { Id = 5, Name = "G.Skill", Country = "Taiwan", Website = "https://www.gskill.com" },
                new Manufacturer { Id = 6, Name = "Samsung", Country = "South Korea", Website = "https://www.samsung.com" },
                new Manufacturer { Id = 7, Name = "WD", Country = "USA", Website = "https://www.westerndigital.com" },
                new Manufacturer { Id = 8, Name = "Seagate", Country = "USA", Website = "https://www.seagate.com" },
                new Manufacturer { Id = 9, Name = "ASUS", Country = "Taiwan", Website = "https://www.asus.com" },
                new Manufacturer { Id = 10, Name = "MSI", Country = "Taiwan", Website = "https://www.msi.com" },
                new Manufacturer { Id = 11, Name = "be quiet!", Country = "Germany", Website = "https://www.bequiet.com" },
                new Manufacturer { Id = 12, Name = "Fractal Design", Country = "Sweden", Website = "https://www.fractal-design.com" },
                new Manufacturer { Id = 13, Name = "NZXT", Country = "USA", Website = "https://www.nzxt.com" },
                new Manufacturer { Id = 14, Name = "Noctua", Country = "Austria", Website = "https://www.noctua.at" }
            );

            var seedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            modelBuilder.Entity<PcPart>().HasData(
                new PcPart { Id = 1, Name = "Intel Core i9-14900K", CategoryId = 1, ManufacturerId = 1, Price = 589.99m, Stock = 15, CreatedAt = seedDate },
                new PcPart { Id = 2, Name = "Intel Core i5-14600K", CategoryId = 1, ManufacturerId = 1, Price = 299.99m, Stock = 30, CreatedAt = seedDate },
                new PcPart { Id = 3, Name = "AMD Ryzen 9 7950X", CategoryId = 1, ManufacturerId = 2, Price = 549.99m, Stock = 12, CreatedAt = seedDate },
                new PcPart { Id = 4, Name = "AMD Ryzen 5 7600X", CategoryId = 1, ManufacturerId = 2, Price = 229.99m, Stock = 40, CreatedAt = seedDate },
                new PcPart { Id = 5, Name = "NVIDIA GeForce RTX 4090", CategoryId = 2, ManufacturerId = 3, Price = 1599.99m, Stock = 8, CreatedAt = seedDate },
                new PcPart { Id = 6, Name = "NVIDIA GeForce RTX 4070", CategoryId = 2, ManufacturerId = 3, Price = 599.99m, Stock = 20, CreatedAt = seedDate },
                new PcPart { Id = 7, Name = "AMD Radeon RX 7900 XTX", CategoryId = 2, ManufacturerId = 2, Price = 999.99m, Stock = 10, CreatedAt = seedDate },
                new PcPart { Id = 8, Name = "AMD Radeon RX 7600", CategoryId = 2, ManufacturerId = 2, Price = 269.99m, Stock = 25, CreatedAt = seedDate },
                new PcPart { Id = 9, Name = "Corsair Vengeance 32GB DDR5-6000", CategoryId = 3, ManufacturerId = 4, Price = 139.99m, Stock = 50, CreatedAt = seedDate },
                new PcPart { Id = 10, Name = "G.Skill Trident Z5 16GB DDR5-5600", CategoryId = 3, ManufacturerId = 5, Price = 79.99m, Stock = 60, CreatedAt = seedDate },
                new PcPart { Id = 11, Name = "Samsung 990 Pro 1TB NVMe", CategoryId = 4, ManufacturerId = 6, Price = 109.99m, Stock = 45, CreatedAt = seedDate },
                new PcPart { Id = 12, Name = "WD Black SN850X 2TB NVMe", CategoryId = 4, ManufacturerId = 7, Price = 179.99m, Stock = 30, CreatedAt = seedDate },
                new PcPart { Id = 13, Name = "Seagate Barracuda 4TB HDD", CategoryId = 4, ManufacturerId = 8, Price = 74.99m, Stock = 35, CreatedAt = seedDate },
                new PcPart { Id = 14, Name = "ASUS ROG Maximus Z790 Hero", CategoryId = 5, ManufacturerId = 9, Price = 599.99m, Stock = 10, CreatedAt = seedDate },
                new PcPart { Id = 15, Name = "MSI MAG B650 TOMAHAWK WIFI", CategoryId = 5, ManufacturerId = 10, Price = 229.99m, Stock = 22, CreatedAt = seedDate },
                new PcPart { Id = 16, Name = "Corsair RM1000x 1000W 80+ Gold", CategoryId = 6, ManufacturerId = 4, Price = 189.99m, Stock = 20, CreatedAt = seedDate },
                new PcPart { Id = 17, Name = "be quiet! Straight Power 850W", CategoryId = 6, ManufacturerId = 11, Price = 149.99m, Stock = 25, CreatedAt = seedDate },
                new PcPart { Id = 18, Name = "Fractal Design Torrent", CategoryId = 7, ManufacturerId = 12, Price = 179.99m, Stock = 15, CreatedAt = seedDate },
                new PcPart { Id = 19, Name = "NZXT H510 Flow", CategoryId = 7, ManufacturerId = 13, Price = 99.99m, Stock = 20, CreatedAt = seedDate },
                new PcPart { Id = 20, Name = "Noctua NH-D15", CategoryId = 8, ManufacturerId = 14, Price = 99.99m, Stock = 30, CreatedAt = seedDate },
                new PcPart { Id = 21, Name = "Corsair H150i Elite LCD", CategoryId = 8, ManufacturerId = 4, Price = 259.99m, Stock = 18, CreatedAt = seedDate }
            );

            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 1,
                Username = "admin",
                PasswordHash = HashPassword("Admin1234"),
                Email = "admin@computerstore.bg",
                Role = UserRole.Admin,
                CreatedAt = seedDate
            });
        }

        private static string HashPassword(string password) =>
            Convert.ToBase64String(
                System.Security.Cryptography.SHA256.HashData(
                    System.Text.Encoding.UTF8.GetBytes(password)));
    }
}