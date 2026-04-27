// ══════════════════════════════════════════════════════════════════════
// OOP: OrdersControl → BaseControl → UserControl
// ══════════════════════════════════════════════════════════════════════
using ComputerStore.Infrastructure;
using ComputerStore.Models;
using ComputerStore.Services.Interfaces;

namespace ComputerStore;

public partial class OrdersControl : BaseControl
{
    private readonly IOrderService _orders = ServiceLocator.Orders;

    public OrdersControl()
    {
        InitializeComponent();
        LoadData();
    }

    // ── OOP: ABSTRACTION ─────────────────────────────────────────────
    public override void LoadData() => LoadOrders();

    // ── Data ──────────────────────────────────────────────────────────
    private void LoadOrders()
    {
        try
        {
            var orders = _orders.GetOrders(Session.CurrentUser!.Id, Session.IsAdmin);

            // Typed OrderRow — SelectionChanged uses pattern-match, no dynamic
            gridOrders.DataSource = orders.Select(o => new OrderRow(
                o.Id,
                o.OrderDate.ToLocalTime().ToString("dd-MM-yyyy HH:mm"),
                o.Status.ToString(),
                o.TotalPrice.ToString("C"),
                o.User.Username)).ToList();

            if (gridOrders.Columns.Contains("Id"))
                gridOrders.Columns["Id"]!.Visible = false;

            if (!Session.IsAdmin && gridOrders.Columns.Contains("User"))
                gridOrders.Columns["User"]!.Visible = false;

            lblStatus.Text = $"{orders.Count} order(s).";
        }
        catch (Exception ex) { lblStatus.Text = ex.Message; }
    }

    private void LoadOrderItems(int orderId)
    {
        try
        {
            var items = _orders.GetOrderItems(orderId);
            gridItems.DataSource = items.Select(oi => new OrderItemRow(
                oi.PcPart.Name,
                oi.Quantity,
                oi.UnitPrice.ToString("C"),
                (oi.UnitPrice * oi.Quantity).ToString("C"))).ToList();
        }
        catch { }
    }

    // ── Events ────────────────────────────────────────────────────────
    private void GridOrders_SelectionChanged(object sender, EventArgs e)
    {
        if (gridOrders.CurrentRow?.DataBoundItem is OrderRow row)
            LoadOrderItems(row.Id);
    }

    private void BtnCancel_Click(object sender, EventArgs e)
    {
        if (gridOrders.CurrentRow?.DataBoundItem is not OrderRow row) return;

        if (row.Status != "Pending")
        { ShowInfo("Only Pending orders can be cancelled."); return; }

        if (!Confirm("Cancel this order and restore stock?")) return;

        try
        {
            _orders.CancelOrder(row.Id);
            LoadOrders();
            gridItems.DataSource = null;
            ShowInfo("Order cancelled and stock restored.");
        }
        catch (Exception ex) { ShowError(ex.Message); }
    }
}
