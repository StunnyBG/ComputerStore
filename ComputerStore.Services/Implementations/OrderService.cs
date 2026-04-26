using ComputerStore.Data;
using ComputerStore.Data.Models;
using ComputerStore.Data.Models.Enums;
using ComputerStore.Services.Dtos;
using ComputerStore.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ComputerStore.Services.Implementations;

public class OrderService : IOrderService
{
    public List<Order> GetOrders(int userId, bool isAdmin)
    {
        using var ctx = DbContextFactory.Create();
        var query = ctx.Orders.Include(o => o.User).AsNoTracking().AsQueryable();

        if (!isAdmin)
            query = query.Where(o => o.UserId == userId);

        return query.OrderByDescending(o => o.OrderDate).ToList();
    }

    public List<OrderItem> GetOrderItems(int orderId)
    {
        using var ctx = DbContextFactory.Create();
        return ctx.OrderItems
                  .Include(oi => oi.PcPart)
                  .AsNoTracking()
                  .Where(oi => oi.OrderId == orderId)
                  .ToList();
    }

    public Order PlaceOrder(int userId, IReadOnlyList<OrderLineDto> lines)
    {
        using var ctx = DbContextFactory.Create();
        using var tx  = ctx.Database.BeginTransaction();

        // ── DATA STRUCTURE: Queue<OrderLineDto> ──────────────────────────
        // REQUIREMENT: Queue is the 4th demonstrated data structure.
        // Items are validated in one pass (without dequeuing) then inserted
        // in FIFO order via Dequeue(), ensuring consistent processing order.
        var orderQueue = new Queue<OrderLineDto>(lines);

        // Validation pass — iterate without removing from the queue
        foreach (var line in orderQueue)
        {
            var part = ctx.PcParts.Find(line.PartId)
                       ?? throw new InvalidOperationException($"Part '{line.Name}' not found.");

            if (part.Stock < line.Quantity)
                throw new InvalidOperationException(
                    $"Insufficient stock for '{line.Name}'. " +
                    $"Requested: {line.Quantity}, available: {part.Stock}.");
        }

        decimal total = lines.Sum(l => l.UnitPrice * l.Quantity);

        var order = new Order
        {
            UserId     = userId,
            OrderDate  = DateTime.UtcNow,
            TotalPrice = total,
        };
        ctx.Orders.Add(order);
        ctx.SaveChanges();

        // Insertion pass — Dequeue() removes each item in FIFO order
        while (orderQueue.Count > 0)
        {
            var line = orderQueue.Dequeue();

            ctx.OrderItems.Add(new OrderItem
            {
                OrderId   = order.Id,
                PcPartId  = line.PartId,
                Quantity  = line.Quantity,
                UnitPrice = line.UnitPrice,
            });

            ctx.PcParts.Find(line.PartId)!.Stock -= line.Quantity;
        }

        ctx.SaveChanges();
        tx.Commit();

        return order;
    }

    public void CancelOrder(int orderId)
    {
        using var ctx = DbContextFactory.Create();
        var order = ctx.Orders
                       .Include(o => o.OrderItems)
                       .ThenInclude(oi => oi.PcPart)
                       .First(o => o.Id == orderId);

        order.Status = OrderStatus.Cancelled;

        foreach (var oi in order.OrderItems)
            oi.PcPart.Stock += oi.Quantity;

        ctx.SaveChanges();
    }
}
