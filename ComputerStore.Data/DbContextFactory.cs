using Microsoft.EntityFrameworkCore;
using static ComputerStore.Common.ApplicationConstants;

namespace ComputerStore.Data;

/// <summary>
/// Creates a fresh <see cref="ComputerStoreDbContext"/> on demand.
/// Lives in the Data project so both the Services project and the
/// WinForms host can reference it without a circular dependency.
/// </summary>
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
