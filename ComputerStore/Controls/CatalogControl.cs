using ComputerStore.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ComputerStore
{
    public partial class CatalogControl : UserControl
    {
        public static List<CartItem> Cart { get; } = new();

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
            catch { /* silently ignore on startup */ }
        }

        private void LoadParts(string search = "", int? categoryId = null)
        {
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

                grid.DataSource = parts.Select(p => new
                {
                    p.Id,
                    p.Name,
                    Category     = p.Category.Name,
                    Manufacturer = p.Manufacturer.Name,
                    Price        = p.Price.ToString("C"),
                    InStock      = p.Stock > 0 ? $"✔ {p.Stock}" : "✘ Out",
                }).ToList();

                if (grid.Columns.Contains("Id")) grid.Columns["Id"]!.Visible = false;

                lblStatus.Text      = $"{parts.Count} part(s) found.";
                lblStatus.ForeColor = AppColors.TextDark;
            }
            catch (Exception ex) { lblStatus.Text = $"Error: {ex.Message}"; }
        }

        // ── Events ────────────────────────────────────────────────────
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
            if (grid.CurrentRow?.DataBoundItem is null) return;

            dynamic row  = grid.CurrentRow.DataBoundItem!;
            int     id   = (int)row.Id;
            string  name = (string)row.Name;

            try
            {
                using var ctx  = DbContextFactory.Create();
                var       part = ctx.PcParts.Find(id);
                if (part is null) return;

                if (part.Stock == 0)
                { MessageBox.Show("This part is out of stock.", "Add to Cart", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

                var existing = Cart.FirstOrDefault(ci => ci.PartId == id);
                if (existing is not null)
                {
                    if (existing.Quantity >= part.Stock)
                    { MessageBox.Show("Maximum available stock already in cart.", "Add to Cart"); return; }
                    existing.Quantity++;
                }
                else
                {
                    Cart.Add(new CartItem { PartId = id, Name = part.Name, UnitPrice = part.Price, Quantity = 1 });
                }

                lblStatus.Text      = $"✔ '{name}' added to cart.";
                lblStatus.ForeColor = AppColors.SuccessGreen;
            }
            catch (Exception ex) { lblStatus.Text = ex.Message; }
        }

        private void BtnDetails_Click(object sender, EventArgs e)
        {
            if (grid.CurrentRow?.DataBoundItem is null) return;
            dynamic row = grid.CurrentRow.DataBoundItem!;
            int     id  = (int)row.Id;

            try
            {
                using var ctx = DbContextFactory.Create();
                var part = ctx.PcParts
                              .Include(p => p.Category)
                              .Include(p => p.Manufacturer)
                              .AsNoTracking()
                              .First(p => p.Id == id);

                string info =
                    $"Name:         {part.Name}\n"         +
                    $"Category:     {part.Category.Name}\n" +
                    $"Manufacturer: {part.Manufacturer.Name}\n" +
                    $"Price:        {part.Price:C}\n"       +
                    $"Stock:        {part.Stock}\n\n"       +
                    $"Description:\n{part.Description ?? "(none)"}";

                MessageBox.Show(info, "Part Details", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch { }
        }

        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; BtnSearch_Click(sender, e); } }
    }
}
