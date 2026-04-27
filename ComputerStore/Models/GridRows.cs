namespace ComputerStore.Models;

// ══════════════════════════════════════════════════════════════════════
// TYPED GRID VIEW-MODELS
//
// Every DataGridView is bound to a List<SomeRow> instead of an
// anonymous type.  This lets SelectionChanged do a clean cast:
//
//   if (grid.CurrentRow?.DataBoundItem is PartRow row)
//       _selectedId = row.Id;
//
// No dynamic, no RuntimeBinderException, full IntelliSense.
// ══════════════════════════════════════════════════════════════════════

public record PartRow(
    int    Id,
    string Name,
    string Category,
    string Manufacturer,
    string Price,
    string InStock);

public record CategoryRow(
    int     Id,
    string  Name,
    string? Description);

public record ManufacturerRow(
    int     Id,
    string  Name,
    string? Country,
    string? Website);

public record UserRow(
    int    Id,
    string Username,
    string Email,
    string Role,
    string Joined);

public record OrderRow(
    int    Id,
    string Date,
    string Status,
    string Total,
    string User);

public record OrderItemRow(
    string Part,
    int    Qty,
    string UnitPrice,
    string Total);

public record CartRow(
    int    PartId,
    string Name,
    string UnitPrice,
    int    Quantity,
    string Total);
