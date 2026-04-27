using ComputerStore.Services.Dtos;
using ComputerStore.Services.Interfaces;

namespace ComputerStore.Services;

/// <summary>
/// Manages the in-memory shopping cart.
/// Lives in Services/ because it is application state with real logic,
/// not infrastructure plumbing like colors or session management.
///
/// It stays in the WinForms project (not ComputerStore.Services) because
/// it never touches the database — it is pure UI-layer state.
/// </summary>
public class CartService : ICartService
{
    private readonly List<CartItem> _items = new();

    public IReadOnlyList<CartItem> Items => _items.AsReadOnly();
    public decimal Total                 => _items.Sum(i => i.Total);
    public int     Count                 => _items.Count;

    /// <summary>
    /// Adds one unit of a part, or increments an existing line.
    /// Returns false if adding would exceed available <paramref name="stock"/>.
    /// </summary>
    public bool TryAdd(int partId, string name, decimal price, int stock)
    {
        var existing = _items.Find(i => i.PartId == partId);

        if (existing is not null)
        {
            if (existing.Quantity >= stock) return false;
            existing.Quantity++;
            return true;
        }

        _items.Add(new CartItem { PartId = partId, Name = name, UnitPrice = price, Quantity = 1 });
        return true;
    }

    /// <summary>
    /// Changes an item's quantity by <paramref name="delta"/>.
    /// Removes the item if quantity would drop below 1.
    /// Returns false if incrementing would exceed <paramref name="maxStock"/>.
    /// </summary>
    public bool TryAdjust(int partId, int delta, int maxStock = int.MaxValue)
    {
        int idx = _items.FindIndex(i => i.PartId == partId);
        if (idx < 0) return false;

        int newQty = _items[idx].Quantity + delta;

        if (newQty < 1)      { _items.RemoveAt(idx); return true; }
        if (newQty > maxStock) return false;

        _items[idx].Quantity = newQty;
        return true;
    }

    public void Remove(int partId) => _items.RemoveAll(i => i.PartId == partId);
    public void Clear()            => _items.Clear();

    public int GetQuantityInCart(int partId) =>
        _items.Find(i => i.PartId == partId)?.Quantity ?? 0;

    /// <summary>
    /// Converts cart contents to <see cref="OrderLineDto"/> for the service layer.
    /// </summary>
    public IReadOnlyList<OrderLineDto> ToOrderLines() =>
        _items.Select(i => new OrderLineDto(i.PartId, i.Name, i.UnitPrice, i.Quantity))
              .ToList()
              .AsReadOnly();
}
