using ComputerStore.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ComputerStore
{
    public partial class CartControl : UserControl
    {
        private int _selectedPartId = -1;

        public CartControl()
        {
            InitializeComponent();
            RefreshGrid();
        }

        // ── Refresh ───────────────────────────────────────────────────
        private void RefreshGrid()
        {
            // Save before DataSource assignment fires SelectionChanged and wipes it.
            int savedId = _selectedPartId;

            grid.DataSource = CatalogControl.Cart.Select(ci => new
            {
                ci.PartId,
                ci.Name,
                UnitPrice = ci.UnitPrice.ToString("C"),
                ci.Quantity,
                Total = ci.Total.ToString("C"),
            }).ToList();

            if (grid.Columns.Contains("PartId")) grid.Columns["PartId"]!.Visible = false;

            // Restore selection using the saved ID (not _selectedPartId which was reset).
            if (savedId != -1)
            {
                foreach (DataGridViewRow row in grid.Rows)
                {
                    if (row.DataBoundItem is null) continue;
                    dynamic item = row.DataBoundItem!;
                    if ((int)item.PartId != savedId) continue;
                    row.Selected        = true;
                    grid.CurrentCell    = row.Cells[FirstVisibleColumn(grid)];
                    _selectedPartId     = savedId; // re-sync the field
                    break;
                }
            }

            decimal grand = CatalogControl.Cart.Sum(ci => ci.Total);
            lblTotal.Text = $"Grand Total:  {grand:C}";
        }

        // ── Events ────────────────────────────────────────────────────
        private void Grid_SelectionChanged(object sender, EventArgs e)
        {
            if (grid.CurrentRow?.DataBoundItem is null) { _selectedPartId = -1; return; }
            dynamic row = grid.CurrentRow.DataBoundItem!;
            _selectedPartId = (int)row.PartId;
        }

        private void BtnRemove_Click(object sender, EventArgs e)
        {
            if (_selectedPartId == -1) return;
            CatalogControl.Cart.RemoveAll(ci => ci.PartId == _selectedPartId);
            _selectedPartId = -1;
            RefreshGrid();
        }

        private void BtnInc_Click(object sender, EventArgs e) => AdjustQty(+1);
        private void BtnDec_Click(object sender, EventArgs e) => AdjustQty(-1);

        private void AdjustQty(int delta)
        {
            if (_selectedPartId == -1) return;
            var item = CatalogControl.Cart.FirstOrDefault(ci => ci.PartId == _selectedPartId);
            if (item is null) return;

            int newQty = item.Quantity + delta;
            if (newQty < 1)
            {
                CatalogControl.Cart.Remove(item);
                _selectedPartId = -1;
            }
            else
            {
                if (delta > 0)
                {
                    try
                    {
                        using var ctx = DbContextFactory.Create();
                        var part = ctx.PcParts.Find(_selectedPartId);
                        if (part is not null && newQty > part.Stock)
                        {
                            MessageBox.Show(
                                $"Cannot add more '{item.Name}'.\nOnly {part.Stock} unit(s) available in stock.",
                                "Insufficient Stock",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                            return;
                        }
                    }
                    catch { }
                }
                item.Quantity = newQty;
            }
            RefreshGrid();
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            if (CatalogControl.Cart.Count == 0) return;
            if (MessageBox.Show("Clear the entire cart?", "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                CatalogControl.Cart.Clear();
                _selectedPartId = -1;
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
                ctx.SaveChanges();

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
                _selectedPartId = -1;
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
        // Returns the index of the first visible column so we never try to
        // set CurrentCell to a hidden column (which throws InvalidOperationException).
        private static int FirstVisibleColumn(DataGridView g)
        {
            foreach (DataGridViewColumn col in g.Columns)
                if (col.Visible) return col.Index;
            return 0;
        }
    }
}
