using ComputerStore.Data;
using Microsoft.EntityFrameworkCore;
using static ComputerStore.Common.ApplicationConstants;

namespace ComputerStore
{
    public static class DbContextFactory
    {
        public static ComputerStoreDbContext Create()
        {
            var opts = new DbContextOptionsBuilder<ComputerStoreDbContext>()
                           .UseSqlite(ConnectionString)
                           .Options;
            return new ComputerStoreDbContext(opts);
        }
    }
}
