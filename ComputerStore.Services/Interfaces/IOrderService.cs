using ComputerStore.Data.Models;
using ComputerStore.Services.Dtos;

namespace ComputerStore.Services.Interfaces;

public interface IOrderService
{
    List<Order>     GetOrders(int userId, bool isAdmin);
    List<OrderItem> GetOrderItems(int orderId);

    /// <summary>
    /// Places a new order.  Validates stock, persists order + items,
    /// and decrements stock — all in one transaction.
    /// Throws <see cref="InvalidOperationException"/> if stock is insufficient.
    /// </summary>
    Order PlaceOrder(int userId, IReadOnlyList<OrderLineDto> lines);

    /// <summary>Cancels an order and restores stock for each item.</summary>
    void CancelOrder(int orderId);
}
