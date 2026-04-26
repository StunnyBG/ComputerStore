// ══════════════════════════════════════════════════════════════════════
// OOP:       CatalogControl → BaseControl → UserControl (3-level chain)
// ALGORITHMS: BinarySearch, SequentialSearchAll, BubbleSort
// DATA:      List<PcPart> from PartService; cart state in CartService
// ══════════════════════════════════════════════════════════════════════
using ComputerStore.Data.Models;
using ComputerStore.Infrastructure;

namespace ComputerStore;

public partial class CatalogControl : BaseControl
{
    private readonly IPartService _parts = ServiceLocator.Parts;
    private readonly CartService  _cart  = ServiceLocator.Cart;

    private int    _selectedPartId  = -1;
    private string _lastSearch      = string.Empty;
    private int?   _lastCategoryId  = null;

    public CatalogControl()
    {
        InitializeComponent();
        LoadCategories();
        LoadData();
    }

    // ── OOP: ABSTRACTION ─────────────────────────────────────────────
    public override void LoadData() => LoadParts();

    // ── OOP: POLYMORPHISM — extends base to also refresh dropdowns ───
    public override void RefreshView()
    {
        LoadCategories();
        LoadParts(_lastSearch, _lastCategoryId);
    }

    // ── Data ──────────────────────────────────────────────────────────
    private void LoadCategories()
    {
        try
        {
            var cats = _parts.GetCategories();
            cmbCategory.Items.Clear();
            cmbCategory.Items.Add("All Categories");
            foreach (var c in cats) cmbCategory.Items.Add(c);
            cmbCategory.DisplayMember = "Name";
            cmbCategory.SelectedIndex = 0;
        }
        catch { /* non-fatal */ }
    }

    private void LoadParts(string search = "", int? categoryId = null)
    {
        _lastSearch     = search;
        _lastCategoryId = categoryId;
        int savedId     = _selectedPartId;

        try
        {
            // Service returns parts sorted by Name — required for binary search.
            var allParts = _parts.GetAll(categoryId);
            List<PcPart> parts;

            if (!string.IsNullOrWhiteSpace(search))
            {
                // ALGORITHM 2: Binary Search — O(log n) exact name match
                int exactIdx = Algorithms.BinarySearch(allParts, search, p => p.Name);

                if (exactIdx >= 0)
                {
                    parts = new List<PcPart> { allParts[exactIdx] };
                }
                else
                {
                    // ALGORITHM 1: Sequential Search — O(n) substring match
                    var indices = Algorithms.SequentialSearchAll(
                        allParts,
                        p => p.Name.Contains(search, StringComparison.OrdinalIgnoreCase)
                          || (p.Description?.Contains(search, StringComparison.OrdinalIgnoreCase)
                              ?? false));

                    parts = new List<PcPart>(indices.Count);
                    foreach (int idx in indices) parts.Add(allParts[idx]);
                }
            }
            else
            {
                parts = allParts;
            }

            // ALGORITHM 3: Bubble Sort — by Category then Name
            Algorithms.BubbleSort(parts, (a, b) =>
            {
                int cmp = string.Compare(
                    a.Category.Name, b.Category.Name, StringComparison.OrdinalIgnoreCase);
                return cmp != 0
                    ? cmp
                    : string.Compare(a.Name, b.Name, StringComparison.OrdinalIgnoreCase);
            });

            // Bind typed PartRow — no dynamic in SelectionChanged
            grid.DataSource = parts.Select(p =>
            {
                int available = p.Stock - _cart.GetQuantityInCart(p.Id);
                return new PartRow(
                    p.Id, p.Name,
                    p.Category.Name,
                    p.Manufacturer.Name,
                    p.Price.ToString("C"),
                    available > 0 ? $"✔ {available}" : "✘ Out");
            }).ToList();

            if (grid.Columns.Contains("Id")) grid.Columns["Id"]!.Visible = false;

            RestoreGridSelection(savedId);
            lblStatus.Text      = $"{parts.Count} part(s) found.";
            lblStatus.ForeColor = AppColors.TextDark;
        }
        catch (Exception ex) { lblStatus.Text = $"Error: {ex.Message}"; }
    }

    // ── Events ────────────────────────────────────────────────────────
    private void Grid_SelectionChanged(object sender, EventArgs e)
    {
        _selectedPartId = grid.CurrentRow?.DataBoundItem is PartRow row ? row.Id : -1;
    }

    private void BtnSearch_Click(object sender, EventArgs e)
    {
        int? catId = cmbCategory.SelectedItem is Category c ? c.Id : (int?)null;
        LoadParts(txtSearch.Text.Trim(), catId);
    }

    private void BtnClear_Click(object sender, EventArgs e)
    {
        txtSearch.Clear();
        cmbCategory.SelectedIndex = 0;
        LoadParts();
    }

    private void BtnAddCart_Click(object sender, EventArgs e)
    {
        if (_selectedPartId == -1) { ShowInfo("Select a part first."); return; }

        try
        {
            var part = _parts.GetById(_selectedPartId);
            if (part is null) return;

            if (!_cart.TryAdd(part.Id, part.Name, part.Price, part.Stock))
            {
                MessageBox.Show("No more stock available for this part.", "Add to Cart",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            lblStatus.Text      = $"✔ '{part.Name}' added to cart.";
            lblStatus.ForeColor = AppColors.SuccessGreen;
            LoadParts(_lastSearch, _lastCategoryId);
        }
        catch (Exception ex) { lblStatus.Text = ex.Message; }
    }

    private void BtnDetails_Click(object sender, EventArgs e)
    {
        if (_selectedPartId == -1) { ShowInfo("Select a part first."); return; }

        try
        {
            var p = _parts.GetById(_selectedPartId);
            if (p is null) return;
            MessageBox.Show(
                $"Name:         {p.Name}\n"             +
                $"Category:     {p.Category.Name}\n"    +
                $"Manufacturer: {p.Manufacturer.Name}\n"+
                $"Price:        {p.Price:C}\n"          +
                $"Stock:        {p.Stock}\n\n"          +
                $"Description:\n{p.Description ?? "(none)"}",
                "Part Details", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch { }
    }

    private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
    { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; BtnSearch_Click(sender, e); } }

    // ── Helpers ──────────────────────────────────────────────────────
    private void RestoreGridSelection(int savedId)
    {
        if (savedId == -1) return;
        foreach (DataGridViewRow row in grid.Rows)
        {
            if (row.DataBoundItem is not PartRow item || item.Id != savedId) continue;
            row.Selected     = true;
            grid.CurrentCell = row.Cells[FirstVisible(grid)];
            _selectedPartId  = savedId;
            break;
        }
    }

    private static int FirstVisible(DataGridView g)
    {
        foreach (DataGridViewColumn c in g.Columns)
            if (c.Visible) return c.Index;
        return 0;
    }
}
