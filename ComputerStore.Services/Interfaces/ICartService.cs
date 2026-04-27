// ComputerStore.Services/Interfaces/ICartService.cs
using ComputerStore.Services.Dtos;

namespace ComputerStore.Services.Interfaces;

public interface ICartService
{
    IReadOnlyList<CartItem> Items { get; }
    decimal Total { get; }
    int Count { get; }

    /// <summary>
    /// Adds one unit of a part, or increments an existing line.
    /// Returns false if adding would exceed available stock.
    /// </summary>
    bool TryAdd(int partId, string name, decimal price, int stock);

    /// <summary>
    /// Changes an item's quantity by delta.
    /// Removes the item if quantity drops below 1.
    /// Returns false if incrementing would exceed maxStock.
    /// </summary>
    bool TryAdjust(int partId, int delta, int maxStock = int.MaxValue);

    void Remove(int partId);
    void Clear();

    int GetQuantityInCart(int partId);

    /// <summary>Converts cart contents to OrderLineDtos for the service layer.</summary>
    IReadOnlyList<OrderLineDto> ToOrderLines();
}