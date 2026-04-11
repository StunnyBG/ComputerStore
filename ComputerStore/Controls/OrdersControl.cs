using ComputerStore.Data.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace ComputerStore
{
    public partial class OrdersControl : UserControl
    {
        public OrdersControl()
        {
            InitializeComponent();
            LoadOrders();
        }

        // ── Data ──────────────────────────────────────────────────────
        private void LoadOrders()
        {
            try
            {
                using var ctx = DbContextFactory.Create();
                var query = ctx.Orders.Include(o => o.User).AsNoTracking().AsQueryable();

                if (!Session.IsAdmin)
                    query = query.Where(o => o.UserId == Session.CurrentUser!.Id);

                var orders = query.OrderByDescending(o => o.OrderDate).ToList();

                gridOrders.DataSource = orders.Select(o => new
                {
                    o.Id,
                    Date   = o.OrderDate.ToLocalTime().ToString("dd-MM-yyyy HH:mm"),
                    Status = o.Status.ToString(),
                    Total  = o.TotalPrice.ToString("C"),
                    User   = o.User.Username,
                }).ToList();

                if (gridOrders.Columns.Contains("Id"))   gridOrders.Columns["Id"]!.Visible   = false;
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

        // ── Events ────────────────────────────────────────────────────
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
            { MessageBox.Show("Only Pending orders can be cancelled.", "Cancel Order"); return; }

            if (MessageBox.Show("Cancel this order and restore stock?", "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

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
                MessageBox.Show("Order cancelled and stock restored.", "Done",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Error"); }
        }
    }
}
