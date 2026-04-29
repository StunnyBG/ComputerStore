using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using static ComputerStore.Common.ApplicationConstants;

namespace ComputerStore.Data;

/// <summary>
/// Provides a design-time factory for creating instances of <see cref="ComputerStoreDbContext"/>.
/// This is used by Entity Framework Core tools (e.g., migrations) when the application's
/// dependency injection configuration is not available.
/// </summary>
public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ComputerStoreDbContext>
{
    public ComputerStoreDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ComputerStoreDbContext>();

        optionsBuilder.UseSqlite(ConnectionString);

        return new ComputerStoreDbContext(optionsBuilder.Options);
    }
}