using ComputerStore.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ComputerStore
{
    public partial class CartControl : UserControl
    {
        public CartControl()
        {
            InitializeComponent();
            RefreshGrid();
        }

        // ── Refresh ───────────────────────────────────────────────────
        private void RefreshGrid()
        {
            grid.DataSource = CatalogControl.Cart.Select(ci => new
            {
                ci.PartId,
                ci.Name,
                UnitPrice = ci.UnitPrice.ToString("C"),
                ci.Quantity,
                Total = ci.Total.ToString("C"),
            }).ToList();

            if (grid.Columns.Contains("PartId")) grid.Columns["PartId"]!.Visible = false;

            decimal grand = CatalogControl.Cart.Sum(ci => ci.Total);
            lblTotal.Text = $"Grand Total:  {grand:C}";
        }

        // ── Events ────────────────────────────────────────────────────
        private void BtnRemove_Click(object sender, EventArgs e)
        {
            if (!TryGetSelectedId(out int id)) return;
            CatalogControl.Cart.RemoveAll(ci => ci.PartId == id);
            RefreshGrid();
        }

        private void BtnInc_Click(object sender, EventArgs e) => AdjustQty(+1);
        private void BtnDec_Click(object sender, EventArgs e) => AdjustQty(-1);

        private void AdjustQty(int delta)
        {
            if (!TryGetSelectedId(out int id)) return;
            var item = CatalogControl.Cart.FirstOrDefault(ci => ci.PartId == id);
            if (item is null) return;
            item.Quantity = Math.Max(1, item.Quantity + delta);
            RefreshGrid();
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            if (CatalogControl.Cart.Count == 0) return;
            if (MessageBox.Show("Clear the entire cart?", "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                CatalogControl.Cart.Clear();
                RefreshGrid();
            }
        }

        private void BtnOrder_Click(object sender, EventArgs e)
        {
            if (!Session.IsLoggedIn) { MessageBox.Show("Please log in first."); return; }
            if (CatalogControl.Cart.Count == 0)
            { MessageBox.Show("Your cart is empty.", "Order"); return; }

            decimal total = CatalogControl.Cart.Sum(ci => ci.Total);
            if (MessageBox.Show($"Place order for {total:C}?", "Confirm Order",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            try
            {
                using var ctx = DbContextFactory.Create();
                using var tx  = ctx.Database.BeginTransaction();

                // Validate stock before writing anything
                foreach (var ci in CatalogControl.Cart)
                {
                    var part = ctx.PcParts.Find(ci.PartId);
                    if (part is null || part.Stock < ci.Quantity)
                    {
                        MessageBox.Show($"Insufficient stock for '{ci.Name}'.", "Order Failed",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                var order = new Order
                {
                    UserId     = Session.CurrentUser!.Id,
                    OrderDate  = DateTime.UtcNow,
                    TotalPrice = total,
                };
                ctx.Orders.Add(order);
                ctx.SaveChanges(); // get order Id

                foreach (var ci in CatalogControl.Cart)
                {
                    ctx.OrderItems.Add(new OrderItem
                    {
                        OrderId   = order.Id,
                        PcPartId  = ci.PartId,
                        Quantity  = ci.Quantity,
                        UnitPrice = ci.UnitPrice,
                    });
                    ctx.PcParts.Find(ci.PartId)!.Stock -= ci.Quantity;
                }
                ctx.SaveChanges();
                tx.Commit();

                CatalogControl.Cart.Clear();
                RefreshGrid();
                MessageBox.Show("✔ Order placed successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error placing order:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── Helpers ──────────────────────────────────────────────────
        private bool TryGetSelectedId(out int id)
        {
            id = 0;
            if (grid.CurrentRow?.DataBoundItem is null) return false;
            dynamic row = grid.CurrentRow.DataBoundItem!;
            id = (int)row.PartId;
            return true;
        }
    }
}
