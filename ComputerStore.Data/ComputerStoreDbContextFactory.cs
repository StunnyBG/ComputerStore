using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using static ComputerStore.Common.ApplicationConstants;

namespace ComputerStore.Data
{
    public class ComputerStoreDbContextFactory : IDesignTimeDbContextFactory<ComputerStoreDbContext>
    {
        public ComputerStoreDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ComputerStoreDbContext>();
            optionsBuilder.UseSqlite(ConnectionString);
            return new ComputerStoreDbContext(optionsBuilder.Options);
        }
    }
}