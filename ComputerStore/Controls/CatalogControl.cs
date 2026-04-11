using ComputerStore.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ComputerStore
{
    public partial class CatalogControl : UserControl
    {
        public static List<CartItem> Cart { get; } = new();

        private int    _selectedPartId = -1;
        private string _lastSearch     = "";
        private int?   _lastCategoryId = null;

        public CatalogControl()
        {
            InitializeComponent();
            LoadCategories();
            LoadParts();
        }

        // ── Data ──────────────────────────────────────────────────────
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

            // Save before DataSource assignment fires SelectionChanged and wipes it.
            int savedId = _selectedPartId;

            try
            {
                using var ctx = DbContextFactory.Create();
                var query = ctx.PcParts
                               .Include(p => p.Category)
                               .Include(p => p.Manufacturer)
                               .AsNoTracking()
                               .AsQueryable();

                if (!string.IsNullOrWhiteSpace(search))
                    query = query.Where(p => p.Name.Contains(search) ||
                                             (p.Description != null && p.Description.Contains(search)));

                if (categoryId.HasValue)
                    query = query.Where(p => p.CategoryId == categoryId.Value);

                var parts = query.OrderBy(p => p.Category.Name).ThenBy(p => p.Name).ToList();

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

                // Restore selection using the locally-saved ID.
                RestoreGridSelection(savedId);

                lblStatus.Text      = $"{parts.Count} part(s) found.";
                lblStatus.ForeColor = AppColors.TextDark;
            }
            catch (Exception ex) { lblStatus.Text = $"Error: {ex.Message}"; }
        }

        // ── Events ────────────────────────────────────────────────────
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
                MessageBox.Show("Please select a part from the list first.", "Add to Cart",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    MessageBox.Show("No more stock available for this part.", "Add to Cart",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var existing = Cart.FirstOrDefault(ci => ci.PartId == _selectedPartId);
                if (existing is not null)
                    existing.Quantity++;
                else
                    Cart.Add(new CartItem { PartId = part.Id, Name = part.Name, UnitPrice = part.Price, Quantity = 1 });

                lblStatus.Text      = $"✔ '{part.Name}' added to cart.";
                lblStatus.ForeColor = AppColors.SuccessGreen;

                // Refresh so InStock column reflects the new cart quantity.
                LoadParts(_lastSearch, _lastCategoryId);
            }
            catch (Exception ex) { lblStatus.Text = ex.Message; }
        }

        private void BtnDetails_Click(object sender, EventArgs e)
        {
            if (_selectedPartId == -1)
            {
                MessageBox.Show("Please select a part from the list first.", "Details",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
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

                MessageBox.Show(info, "Part Details", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch { }
        }

        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; BtnSearch_Click(sender, e); } }

        // ── Helpers ──────────────────────────────────────────────────
        private void RestoreGridSelection(int savedId)
        {
            if (savedId == -1) return;
            foreach (DataGridViewRow row in grid.Rows)
            {
                if (row.DataBoundItem is null) continue;
                dynamic item = row.DataBoundItem!;
                if ((int)item.Id != savedId) continue;
                row.Selected        = true;
                grid.CurrentCell    = row.Cells[FirstVisibleColumn(grid)];
                _selectedPartId     = savedId; // re-sync the field
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
