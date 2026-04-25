// ══════════════════════════════════════════════════════════════════════
// OOP: CartControl INHERITS from BaseControl
// ══════════════════════════════════════════════════════════════════════
using ComputerStore.Data.Models;
using ComputerStore.Infrastructure;   // BaseControl, Algorithms
using Microsoft.EntityFrameworkCore;

namespace ComputerStore
{
    public partial class CartControl : BaseControl   // INHERITANCE — extends BaseControl
    {
        private int _selectedPartId = -1;

        public CartControl()
        {
            InitializeComponent();
            LoadData();
        }

        // ── OOP: ABSTRACTION — implementing the abstract LoadData contract ─
        public override void LoadData() => RefreshGrid();

        // ── Grid refresh ──────────────────────────────────────────────────
        private void RefreshGrid()
        {
            int savedId = _selectedPartId;

            // ── DATA STRUCTURE: List<CartItem> (defined in CatalogControl) ─
            // The cart is already a List<CartItem>. Here we make a copy and
            // sort it with our custom Bubble Sort before binding to the grid,
            // so the user sees items ordered by total price descending.

            // ALGORITHM 3: Bubble Sort — sort cart items by total (desc)
            var sortedCart = new List<CartItem>(CatalogControl.Cart); // copy
            Algorithms.BubbleSort(sortedCart,
                (a, b) => b.Total.CompareTo(a.Total));  // descending by total

            grid.DataSource = sortedCart.Select(ci => new
            {
                ci.PartId,
                ci.Name,
                UnitPrice = ci.UnitPrice.ToString("C"),
                ci.Quantity,
                Total = ci.Total.ToString("C"),
            }).ToList();

            if (grid.Columns.Contains("PartId")) grid.Columns["PartId"]!.Visible = false;

            // Restore selection
            if (savedId != -1)
            {
                foreach (DataGridViewRow row in grid.Rows)
                {
                    if (row.DataBoundItem is null) continue;
                    dynamic item = row.DataBoundItem!;
                    if ((int)item.PartId != savedId) continue;
                    row.Selected     = true;
                    grid.CurrentCell = row.Cells[FirstVisibleColumn(grid)];
                    _selectedPartId  = savedId;
                    break;
                }
            }

            decimal grand = CatalogControl.Cart.Sum(ci => ci.Total);
            lblTotal.Text = $"Grand Total:  {grand:C}";
        }

        // ── Events ────────────────────────────────────────────────────────
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

            // ALGORITHM 1: Sequential Search — find the item in the cart
            int idx = Algorithms.SequentialSearch(CatalogControl.Cart,
                ci => ci.PartId == _selectedPartId);
            if (idx < 0) return;

            var item   = CatalogControl.Cart[idx];
            int newQty = item.Quantity + delta;

            if (newQty < 1)
            {
                CatalogControl.Cart.RemoveAt(idx);
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
                                $"Cannot add more '{item.Name}'.\n" +
                                $"Only {part.Stock} unit(s) available in stock.",
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
            if (Confirm("Clear the entire cart?"))
            {
                CatalogControl.Cart.Clear();
                _selectedPartId = -1;
                RefreshGrid();
            }
        }

        private void BtnOrder_Click(object sender, EventArgs e)
        {
            if (!Session.IsLoggedIn)  { ShowInfo("Please log in first."); return; }
            if (CatalogControl.Cart.Count == 0) { ShowInfo("Your cart is empty."); return; }

            decimal total = CatalogControl.Cart.Sum(ci => ci.Total);
            if (!Confirm($"Place order for {total:C}?")) return;

            try
            {
                using var ctx = DbContextFactory.Create();
                using var tx  = ctx.Database.BeginTransaction();

                // ── DATA STRUCTURE: Queue<CartItem> ───────────────────────
                // REQUIREMENT: Queue is the 4th data structure demonstrated.
                // We enqueue each cart item for validation, then dequeue them
                // one-by-one for DB insertion.  This guarantees:
                //   • All items are validated BEFORE any DB write begins
                //   • Items are inserted in FIFO order (same as cart order)
                // ─────────────────────────────────────────────────────────
                var orderQueue = new Queue<CartItem>(CatalogControl.Cart);

                // Validation pass — peek at every item without dequeuing
                foreach (var ci in orderQueue)
                {
                    var part = ctx.PcParts.Find(ci.PartId);
                    if (part is null || part.Stock < ci.Quantity)
                    {
                        MessageBox.Show(
                            $"Insufficient stock for '{ci.Name}'.",
                            "Order Failed",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                // Create the order header
                var order = new Order
                {
                    UserId     = Session.CurrentUser!.Id,
                    OrderDate  = DateTime.UtcNow,
                    TotalPrice = total,
                };
                ctx.Orders.Add(order);
                ctx.SaveChanges();

                // Insertion pass — dequeue and persist each item
                while (orderQueue.Count > 0)
                {
                    var ci = orderQueue.Dequeue();   // Queue.Dequeue() — FIFO removal

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
                ShowInfo("✔ Order placed successfully!");
            }
            catch (Exception ex)
            {
                ShowError($"Error placing order:\n{ex.Message}");
            }
        }

        // ── Helpers ──────────────────────────────────────────────────────
        private static int FirstVisibleColumn(DataGridView g)
        {
            foreach (DataGridViewColumn col in g.Columns)
                if (col.Visible) return col.Index;
            return 0;
        }
    }
}
