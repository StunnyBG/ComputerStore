// ══════════════════════════════════════════════════════════════════════
// OOP:      CartControl → BaseControl → UserControl
// ALGORITHM: BubbleSort used to sort cart display; Queue in OrderService
// ══════════════════════════════════════════════════════════════════════
using ComputerStore.Infrastructure;
using ComputerStore.Models;
using ComputerStore.Services.Interfaces;

namespace ComputerStore;

public partial class CartControl : BaseControl
{
    private readonly ICartService _cart = ServiceLocator.Cart;
    private readonly IOrderService _orders = ServiceLocator.Orders;

    private int _selectedPartId = -1;

    public CartControl()
    {
        InitializeComponent();
        LoadData();
    }

    // ── OOP: ABSTRACTION ─────────────────────────────────────────────
    public override void LoadData() => RefreshGrid();

    // ── Grid ──────────────────────────────────────────────────────────
    private void RefreshGrid()
    {
        int savedId = _selectedPartId;

        // Copy and sort by Total descending using BubbleSort (ALGORITHM 3)
        var items = _cart.Items.ToList();
        Algorithms.BubbleSort(items, (a, b) => b.Total.CompareTo(a.Total));

        // Typed CartRow — no dynamic needed in SelectionChanged
        grid.DataSource = items.Select(ci => new CartRow(
            ci.PartId, ci.Name,
            ci.UnitPrice.ToString("C"),
            ci.Quantity,
            ci.Total.ToString("C"))).ToList();

        if (grid.Columns.Contains("PartId")) grid.Columns["PartId"]!.Visible = false;

        // Restore selection after rebind
        if (savedId != -1)
            foreach (DataGridViewRow row in grid.Rows)
            {
                if (row.DataBoundItem is not CartRow item || item.PartId != savedId) continue;
                row.Selected     = true;
                grid.CurrentCell = row.Cells[FirstVisible(grid)];
                _selectedPartId  = savedId;
                break;
            }

        lblTotal.Text = $"Grand Total:  {_cart.Total:C}";
    }

    // ── Events ────────────────────────────────────────────────────────
    private void Grid_SelectionChanged(object sender, EventArgs e)
    {
        _selectedPartId = grid.CurrentRow?.DataBoundItem is CartRow row ? row.PartId : -1;
    }

    private void BtnRemove_Click(object sender, EventArgs e)
    {
        if (_selectedPartId == -1) return;
        _cart.Remove(_selectedPartId);
        _selectedPartId = -1;
        RefreshGrid();
    }

    private void BtnInc_Click(object sender, EventArgs e) => AdjustQty(+1);
    private void BtnDec_Click(object sender, EventArgs e) => AdjustQty(-1);

    private void AdjustQty(int delta)
    {
        if (_selectedPartId == -1) return;

        if (delta > 0)
        {
            try
            {
                var part = ServiceLocator.Parts.GetById(_selectedPartId);
                if (part is not null && !_cart.TryAdjust(_selectedPartId, delta, part.Stock))
                {
                    MessageBox.Show($"Only {part.Stock} unit(s) available in stock.",
                        "Insufficient Stock", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            catch { return; }
        }
        else
        {
            _cart.TryAdjust(_selectedPartId, delta);
            if (_cart.GetQuantityInCart(_selectedPartId) == 0)
                _selectedPartId = -1;
        }

        RefreshGrid();
    }

    private void BtnClear_Click(object sender, EventArgs e)
    {
        if (_cart.Count == 0 || !Confirm("Clear the entire cart?")) return;
        _cart.Clear();
        _selectedPartId = -1;
        RefreshGrid();
    }

    private void BtnOrder_Click(object sender, EventArgs e)
    {
        if (!Session.IsLoggedIn) { ShowInfo("Please log in first."); return; }
        if (_cart.Count == 0)    { ShowInfo("Your cart is empty.");  return; }
        if (!Confirm($"Place order for {_cart.Total:C}?")) return;

        try
        {
            // Queue + DB validation lives inside OrderService.PlaceOrder
            _orders.PlaceOrder(Session.CurrentUser!.Id, _cart.ToOrderLines());
            _cart.Clear();
            _selectedPartId = -1;
            RefreshGrid();
            ShowInfo("✔ Order placed successfully!");
        }
        catch (InvalidOperationException ex)
        {
            MessageBox.Show(ex.Message, "Order Failed",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        catch (Exception ex) { ShowError($"Error placing order:\n{ex.Message}"); }
    }

    // ── Helpers ──────────────────────────────────────────────────────
    private static int FirstVisible(DataGridView g)
    {
        foreach (DataGridViewColumn c in g.Columns)
            if (c.Visible) return c.Index;
        return 0;
    }
}
