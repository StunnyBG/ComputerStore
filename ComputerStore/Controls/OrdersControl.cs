// ══════════════════════════════════════════════════════════════════════
// OOP: OrdersControl INHERITS from BaseControl
// ══════════════════════════════════════════════════════════════════════
using ComputerStore.Data.Models.Enums;
using ComputerStore.Infrastructure;   // BaseControl
using Microsoft.EntityFrameworkCore;

namespace ComputerStore
{
    public partial class OrdersControl : BaseControl   // INHERITANCE — extends BaseControl
    {
        public OrdersControl()
        {
            InitializeComponent();
            LoadData();
        }

        // ── OOP: ABSTRACTION — implementing the abstract LoadData contract ─
        public override void LoadData() => LoadOrders();

        // ── Data ──────────────────────────────────────────────────────────
        private void LoadOrders()
        {
            try
            {
                using var ctx = DbContextFactory.Create();
                var query = ctx.Orders.Include(o => o.User).AsNoTracking().AsQueryable();

                if (!Session.IsAdmin)
                    query = query.Where(o => o.UserId == Session.CurrentUser!.Id);

                // DATA STRUCTURE: List<Order> — holds the result set for display
                var orders = query.OrderByDescending(o => o.OrderDate).ToList();

                gridOrders.DataSource = orders.Select(o => new
                {
                    o.Id,
                    Date   = o.OrderDate.ToLocalTime().ToString("dd-MM-yyyy HH:mm"),
                    Status = o.Status.ToString(),
                    Total  = o.TotalPrice.ToString("C"),
                    User   = o.User.Username,
                }).ToList();

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
                using var ctx = DbContextFactory.Create();
                var items = ctx.OrderItems
                               .Include(oi => oi.PcPart)
                               .AsNoTracking()
                               .Where(oi => oi.OrderId == orderId)
                               .ToList();

                gridItems.DataSource = items.Select(oi => new
                {
                    Part      = oi.PcPart.Name,
                    Qty       = oi.Quantity,
                    UnitPrice = oi.UnitPrice.ToString("C"),
                    Total     = (oi.UnitPrice * oi.Quantity).ToString("C"),
                }).ToList();
            }
            catch { }
        }

        // ── Events ────────────────────────────────────────────────────────
        private void GridOrders_SelectionChanged(object sender, EventArgs e)
        {
            if (gridOrders.CurrentRow?.DataBoundItem is null) return;
            dynamic row = gridOrders.CurrentRow.DataBoundItem!;
            LoadOrderItems((int)row.Id);
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (gridOrders.CurrentRow?.DataBoundItem is null) return;
            dynamic row    = gridOrders.CurrentRow.DataBoundItem!;
            int     id     = (int)row.Id;
            string  status = (string)row.Status;

            if (status != "Pending")
            {
                ShowInfo("Only Pending orders can be cancelled.");
                return;
            }

            if (!Confirm("Cancel this order and restore stock?")) return;

            try
            {
                using var ctx = DbContextFactory.Create();
                var order = ctx.Orders
                               .Include(o => o.OrderItems)
                               .ThenInclude(oi => oi.PcPart)
                               .First(o => o.Id == id);

                order.Status = OrderStatus.Cancelled;
                foreach (var oi in order.OrderItems)
                    oi.PcPart.Stock += oi.Quantity;

                ctx.SaveChanges();
                LoadOrders();
                gridItems.DataSource = null;
                ShowInfo("Order cancelled and stock restored.");
            }
            catch (Exception ex) { ShowError(ex.Message); }
        }
    }
}
