// ══════════════════════════════════════════════════════════════════════
// OOP: AdminControl INHERITS from BaseControl
// ══════════════════════════════════════════════════════════════════════
using ComputerStore.Data.Models;
using ComputerStore.Data.Models.Enums;
using ComputerStore.Infrastructure;   // BaseControl
using Microsoft.EntityFrameworkCore;

namespace ComputerStore
{
    public partial class AdminControl : BaseControl   // INHERITANCE — extends BaseControl
    {
        public AdminControl()
        {
            if (!Session.IsAdmin)
                throw new UnauthorizedAccessException("Admin access required.");
            InitializeComponent();
            LoadData();
        }

        // ── OOP: ABSTRACTION — implementing the abstract LoadData contract ─
        public override void LoadData() => LoadAll();

        // ── Load ──────────────────────────────────────────────────────────
        private void LoadAll() { LoadParts(); LoadCategories(); LoadManufacturers(); LoadUsers(); }

        private void LoadParts()
        {
            try
            {
                using var ctx = DbContextFactory.Create();
                var parts = ctx.PcParts.Include(p => p.Category).Include(p => p.Manufacturer)
                                       .AsNoTracking().OrderBy(p => p.Name).ToList();
                gridParts.DataSource = parts.Select(p => new
                {
                    p.Id, p.Name,
                    Category     = p.Category.Name,
                    Manufacturer = p.Manufacturer.Name,
                    Price        = p.Price.ToString("C"),
                    p.Stock,
                }).ToList();
                HideCol(gridParts, "Id");
                lblPartsStatus.Text = $"{parts.Count} part(s).";
            }
            catch (Exception ex) { lblPartsStatus.Text = ex.Message; }
        }

        private void LoadCategories()
        {
            try
            {
                using var ctx = DbContextFactory.Create();
                var cats = ctx.Categories.AsNoTracking().OrderBy(c => c.Name).ToList();
                gridCats.DataSource = cats.Select(c => new { c.Id, c.Name, c.Description }).ToList();
                HideCol(gridCats, "Id");
                lblCatStatus.Text = $"{cats.Count} categories.";
            }
            catch (Exception ex) { lblCatStatus.Text = ex.Message; }
        }

        private void LoadManufacturers()
        {
            try
            {
                using var ctx = DbContextFactory.Create();
                var mfrs = ctx.Manufacturers.AsNoTracking().OrderBy(m => m.Name).ToList();
                gridMfr.DataSource = mfrs.Select(m => new { m.Id, m.Name, m.Country, m.Website }).ToList();
                HideCol(gridMfr, "Id");
                lblMfrStatus.Text = $"{mfrs.Count} manufacturers.";
            }
            catch (Exception ex) { lblMfrStatus.Text = ex.Message; }
        }

        private void LoadUsers()
        {
            try
            {
                using var ctx = DbContextFactory.Create();
                var users = ctx.Users.AsNoTracking().OrderBy(u => u.Username).ToList();
                gridUsers.DataSource = users.Select(u => new
                {
                    u.Id, u.Username, u.Email,
                    Role   = u.Role.ToString(),
                    Joined = u.CreatedAt.ToLocalTime().ToString("dd-MM-yyyy"),
                }).ToList();
                HideCol(gridUsers, "Id");
                lblUsersStatus.Text = $"{users.Count} users.";
            }
            catch (Exception ex) { lblUsersStatus.Text = ex.Message; }
        }

        // ── Parts CRUD ────────────────────────────────────────────────────
        private void BtnAddPart_Click(object sender, EventArgs e)
        {
            using var dlg = new PartEditForm();
            if (dlg.ShowDialog(this) == DialogResult.OK) LoadParts();
        }

        private void BtnEditPart_Click(object sender, EventArgs e)
        {
            if (!TryGetId(gridParts, out int id)) return;
            using var dlg = new PartEditForm(id);
            if (dlg.ShowDialog(this) == DialogResult.OK) LoadParts();
        }

        private void BtnDeletePart_Click(object sender, EventArgs e)
        {
            if (!TryGetId(gridParts, out int id)) return;
            if (!Confirm("Delete this part?")) return;
            try
            {
                using var ctx = DbContextFactory.Create();
                var p = ctx.PcParts.Find(id);
                if (p is not null) { ctx.PcParts.Remove(p); ctx.SaveChanges(); }
                LoadParts();
            }
            catch (Exception ex) { ShowError(ex.Message); }
        }

        // ── Category CRUD ─────────────────────────────────────────────────
        private void BtnAddCat_Click(object sender, EventArgs e)
        {
            string name = Prompt("Category name:");
            if (string.IsNullOrWhiteSpace(name)) return;
            string desc = Prompt("Description (optional):");
            try
            {
                using var ctx = DbContextFactory.Create();
                ctx.Categories.Add(new Category { Name = name.Trim(), Description = desc });
                ctx.SaveChanges();
                LoadCategories();
            }
            catch (Exception ex) { ShowError(ex.Message); }
        }

        private void BtnDeleteCat_Click(object sender, EventArgs e)
        {
            if (!TryGetId(gridCats, out int id) || !Confirm("Delete this category?")) return;
            try
            {
                using var ctx = DbContextFactory.Create();
                var c = ctx.Categories.Find(id);
                if (c is not null) { ctx.Categories.Remove(c); ctx.SaveChanges(); }
                LoadCategories();
            }
            catch (Exception ex) { ShowError($"Cannot delete: {ex.Message}"); }
        }

        // ── Manufacturer CRUD ─────────────────────────────────────────────
        private void BtnAddMfr_Click(object sender, EventArgs e)
        {
            string name    = Prompt("Manufacturer name:");
            if (string.IsNullOrWhiteSpace(name)) return;
            string country = Prompt("Country:");
            string website = Prompt("Website URL:");
            try
            {
                using var ctx = DbContextFactory.Create();
                ctx.Manufacturers.Add(new Manufacturer
                {
                    Name    = name.Trim(),
                    Country = country,
                    Website = website
                });
                ctx.SaveChanges();
                LoadManufacturers();
            }
            catch (Exception ex) { ShowError(ex.Message); }
        }

        private void BtnDeleteMfr_Click(object sender, EventArgs e)
        {
            if (!TryGetId(gridMfr, out int id) || !Confirm("Delete this manufacturer?")) return;
            try
            {
                using var ctx = DbContextFactory.Create();
                var m = ctx.Manufacturers.Find(id);
                if (m is not null) { ctx.Manufacturers.Remove(m); ctx.SaveChanges(); }
                LoadManufacturers();
            }
            catch (Exception ex) { ShowError($"Cannot delete: {ex.Message}"); }
        }

        // ── Users ─────────────────────────────────────────────────────────
        private void BtnToggleRole_Click(object sender, EventArgs e)
        {
            if (!TryGetId(gridUsers, out int id)) return;
            if (id == Session.CurrentUser!.Id)
            { ShowInfo("You cannot change your own role."); return; }

            try
            {
                using var ctx = DbContextFactory.Create();
                var u = ctx.Users.Find(id)!;
                u.Role = u.Role == UserRole.Admin ? UserRole.Customer : UserRole.Admin;
                ctx.SaveChanges();
                LoadUsers();
            }
            catch (Exception ex) { ShowError(ex.Message); }
        }

        // ── Helpers ──────────────────────────────────────────────────────
        private static void HideCol(DataGridView g, string col)
        { if (g.Columns.Contains(col)) g.Columns[col]!.Visible = false; }

        private static bool TryGetId(DataGridView g, out int id)
        {
            id = 0;
            if (g.CurrentRow?.DataBoundItem is null) return false;
            dynamic row = g.CurrentRow.DataBoundItem!;
            id = (int)row.Id;
            return true;
        }

        private static string Prompt(string question)
        {
            using var f = new Form
            {
                Width           = 380, Height = 160,
                Text            = question,
                StartPosition   = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox     = false
            };
            var lbl   = new Label  { Text = question, AutoSize = true, Location = new Point(14, 16) };
            var txt   = new TextBox { Location = new Point(14, 38), Width = 340 };
            var btnOk = new Button  { Text = "OK",     DialogResult = DialogResult.OK,     Location = new Point(198, 76), Width = 70 };
            var btnNo = new Button  { Text = "Cancel", DialogResult = DialogResult.Cancel, Location = new Point(278, 76), Width = 70 };
            f.Controls.AddRange(new Control[] { lbl, txt, btnOk, btnNo });
            f.AcceptButton = btnOk;
            f.CancelButton = btnNo;
            return f.ShowDialog() == DialogResult.OK ? txt.Text : string.Empty;
        }
    }
}
