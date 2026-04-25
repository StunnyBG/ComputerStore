// ══════════════════════════════════════════════════════════════════════
// OOP: CatalogControl INHERITS from BaseControl (which inherits UserControl)
//      Demonstrates the full inheritance chain:
//      UserControl ← BaseControl ← CatalogControl
// ══════════════════════════════════════════════════════════════════════
using ComputerStore.Data.Models;
using ComputerStore.Infrastructure;   // BaseControl, Algorithms
using Microsoft.EntityFrameworkCore;

namespace ComputerStore
{
    public partial class CatalogControl : BaseControl  // INHERITANCE — extends BaseControl
    {
        // ── DATA STRUCTURE: List<CartItem> (static = shared across the app) ─
        // This is the shopping cart — a List allows O(1) add, O(n) search,
        // and preserves insertion order (important for the cart UI).
        // REQUIREMENT: List is one of the ≥3 required data structures.
        public static List<CartItem> Cart { get; } = new List<CartItem>();

        private int    _selectedPartId = -1;
        private string _lastSearch     = "";
        private int?   _lastCategoryId = null;

        public CatalogControl()
        {
            InitializeComponent();
            LoadCategories();
            LoadData();   // called via the abstract contract from BaseControl
        }

        // ── OOP: ABSTRACTION — implementing the abstract method ──────────
        // BaseControl demands every view provides LoadData(); this is ours.
        public override void LoadData() => LoadParts();

        // ── OOP: POLYMORPHISM — overriding the virtual RefreshView ───────
        // We extend the base behaviour: reload dropdowns too.
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
                using var ctx = DbContextFactory.Create();
                var cats = ctx.Categories.AsNoTracking().OrderBy(c => c.Name).ToList();
                cmbCategory.Items.Clear();
                cmbCategory.Items.Add("All Categories");
                foreach (var c in cats) cmbCategory.Items.Add(c);
                cmbCategory.DisplayMember = "Name";
                cmbCategory.SelectedIndex = 0;
            }
            catch { }
        }

        private void LoadParts(string search = "", int? categoryId = null)
        {
            _lastSearch     = search;
            _lastCategoryId = categoryId;

            int savedId = _selectedPartId;

            try
            {
                using var ctx = DbContextFactory.Create();

                // Load all parts (with category filter only — text filter done in-memory
                // so we can demonstrate our custom search algorithms below).
                var query = ctx.PcParts
                               .Include(p => p.Category)
                               .Include(p => p.Manufacturer)
                               .AsNoTracking()
                               .AsQueryable();

                if (categoryId.HasValue)
                    query = query.Where(p => p.CategoryId == categoryId.Value);

                // Load into a list sorted by name for binary search compatibility
                var allParts = query.OrderBy(p => p.Name).ToList();

                List<PcPart> parts;

                if (!string.IsNullOrWhiteSpace(search))
                {
                    // ── ALGORITHM CHOICE LOGIC ────────────────────────────
                    // If the user typed an exact name, try Binary Search first
                    // (O(log n)) on the name-sorted list.  If not found,
                    // fall back to Sequential Search (O(n)) which handles
                    // partial / substring matches.
                    // ─────────────────────────────────────────────────────

                    // ALGORITHM 2: Binary Search — exact name match
                    int exactIdx = Algorithms.BinarySearch(
                        allParts,
                        search,
                        p => p.Name);

                    if (exactIdx >= 0)
                    {
                        // Binary search found an exact match — show just that one part
                        parts = new List<PcPart> { allParts[exactIdx] };
                    }
                    else
                    {
                        // ALGORITHM 1: Sequential Search — substring match
                        // Collects the indices of all parts whose name or
                        // description contains the search string.
                        var indices = Algorithms.SequentialSearchAll(
                            allParts,
                            p => p.Name.Contains(search, StringComparison.OrdinalIgnoreCase)
                              || (p.Description != null &&
                                  p.Description.Contains(search, StringComparison.OrdinalIgnoreCase)));

                        // Reconstruct the filtered list from the found indices
                        // DATA STRUCTURE: new List built from sequential-search results
                        parts = new List<PcPart>(indices.Count);
                        foreach (int idx in indices)
                            parts.Add(allParts[idx]);
                    }
                }
                else
                {
                    // No text filter — use all parts (already sorted by name)
                    parts = allParts;
                }

                // ALGORITHM 3: Bubble Sort — sort the display results by
                // Category name first, then by Part name (mirrors the
                // original ordering but uses our own implementation).
                Algorithms.BubbleSort(parts,
                    (a, b) =>
                    {
                        int catCmp = string.Compare(
                            a.Category.Name, b.Category.Name,
                            StringComparison.OrdinalIgnoreCase);
                        return catCmp != 0
                            ? catCmp
                            : string.Compare(a.Name, b.Name,
                                StringComparison.OrdinalIgnoreCase);
                    });

                grid.DataSource = parts.Select(p =>
                {
                    int inCart    = Cart.FirstOrDefault(ci => ci.PartId == p.Id)?.Quantity ?? 0;
                    int available = p.Stock - inCart;
                    return new
                    {
                        p.Id,
                        p.Name,
                        Category     = p.Category.Name,
                        Manufacturer = p.Manufacturer.Name,
                        Price        = p.Price.ToString("C"),
                        InStock      = available > 0 ? $"✔ {available}" : "✘ Out",
                    };
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
            if (grid.CurrentRow?.DataBoundItem is null) { _selectedPartId = -1; return; }
            dynamic row = grid.CurrentRow.DataBoundItem!;
            _selectedPartId = (int)row.Id;
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            int? catId = cmbCategory.SelectedItem is Category c ? c.Id : null;
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
            if (_selectedPartId == -1)
            {
                ShowInfo("Please select a part from the list first.");
                return;
            }

            try
            {
                using var ctx  = DbContextFactory.Create();
                var       part = ctx.PcParts.Find(_selectedPartId);
                if (part is null) return;

                int inCart    = Cart.FirstOrDefault(ci => ci.PartId == _selectedPartId)?.Quantity ?? 0;
                int available = part.Stock - inCart;

                if (available <= 0)
                {
                    // ALGORITHM 1 (sequential search) used to find existing cart item
                    int cartIdx = Algorithms.SequentialSearch(Cart, ci => ci.PartId == _selectedPartId);
                    MessageBox.Show("No more stock available for this part.", "Add to Cart",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // ALGORITHM 1: Sequential Search — find existing cart entry
                int existingIdx = Algorithms.SequentialSearch(Cart, ci => ci.PartId == _selectedPartId);
                if (existingIdx >= 0)
                    Cart[existingIdx].Quantity++;
                else
                    Cart.Add(new CartItem
                    {
                        PartId    = part.Id,
                        Name      = part.Name,
                        UnitPrice = part.Price,
                        Quantity  = 1
                    });

                lblStatus.Text      = $"✔ '{part.Name}' added to cart.";
                lblStatus.ForeColor = AppColors.SuccessGreen;

                LoadParts(_lastSearch, _lastCategoryId);
            }
            catch (Exception ex) { lblStatus.Text = ex.Message; }
        }

        private void BtnDetails_Click(object sender, EventArgs e)
        {
            if (_selectedPartId == -1)
            {
                ShowInfo("Please select a part from the list first.");
                return;
            }

            try
            {
                using var ctx = DbContextFactory.Create();
                var part = ctx.PcParts
                              .Include(p => p.Category)
                              .Include(p => p.Manufacturer)
                              .AsNoTracking()
                              .First(p => p.Id == _selectedPartId);

                string info =
                    $"Name:         {part.Name}\n"              +
                    $"Category:     {part.Category.Name}\n"      +
                    $"Manufacturer: {part.Manufacturer.Name}\n"  +
                    $"Price:        {part.Price:C}\n"            +
                    $"Stock:        {part.Stock}\n\n"            +
                    $"Description:\n{part.Description ?? "(none)"}";

                MessageBox.Show(info, "Part Details",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                if (row.DataBoundItem is null) continue;
                dynamic item = row.DataBoundItem!;
                if ((int)item.Id != savedId) continue;
                row.Selected     = true;
                grid.CurrentCell = row.Cells[FirstVisibleColumn(grid)];
                _selectedPartId  = savedId;
                break;
            }
        }

        private static int FirstVisibleColumn(DataGridView g)
        {
            foreach (DataGridViewColumn col in g.Columns)
                if (col.Visible) return col.Index;
            return 0;
        }
    }
}
