// ══════════════════════════════════════════════════════════════════════
// OOP: AdminControl → BaseControl → UserControl
// FIX: no constructor throw; typed TryGetId<TRow> replaces dynamic casts
// ══════════════════════════════════════════════════════════════════════
using ComputerStore.Infrastructure;
using ComputerStore.Models;
using ComputerStore.Services.Interfaces;

namespace ComputerStore;

public partial class AdminControl : BaseControl
{
    private readonly IPartService _parts = ServiceLocator.Parts;
    private readonly IUserService _users = ServiceLocator.Users;

    public AdminControl()
    {
        // Guard without throwing — the button is already hidden for non-admins.
        if (!Session.IsAdmin) return;
        InitializeComponent();
        LoadData();
    }

    // ── OOP: ABSTRACTION ─────────────────────────────────────────────
    public override void LoadData() => LoadAll();

    private void LoadAll() { LoadParts(); LoadCategories(); LoadManufacturers(); LoadUsers(); }

    // ── Load ──────────────────────────────────────────────────────────
    private void LoadParts()
    {
        try
        {
            var parts = _parts.GetAll();
            gridParts.DataSource = parts.Select(p => new PartRow(
                p.Id, p.Name,
                p.Category.Name,
                p.Manufacturer.Name,
                p.Price.ToString("C"),
                p.Stock.ToString())).ToList();
            HideCol(gridParts, "Id");
            if (gridParts.Columns.Contains("InStock"))
                gridParts.Columns["InStock"]!.HeaderText = "Stock";
            lblPartsStatus.Text = $"{parts.Count} part(s).";
        }
        catch (Exception ex) { lblPartsStatus.Text = ex.Message; }
    }

    private void LoadCategories()
    {
        try
        {
            var cats = _parts.GetCategories();
            gridCats.DataSource = cats.Select(c =>
                new CategoryRow(c.Id, c.Name, c.Description)).ToList();
            HideCol(gridCats, "Id");
            lblCatStatus.Text = $"{cats.Count} categories.";
        }
        catch (Exception ex) { lblCatStatus.Text = ex.Message; }
    }

    private void LoadManufacturers()
    {
        try
        {
            var mfrs = _parts.GetManufacturers();
            gridMfr.DataSource = mfrs.Select(m =>
                new ManufacturerRow(m.Id, m.Name, m.Country, m.Website)).ToList();
            HideCol(gridMfr, "Id");
            lblMfrStatus.Text = $"{mfrs.Count} manufacturers.";
        }
        catch (Exception ex) { lblMfrStatus.Text = ex.Message; }
    }

    private void LoadUsers()
    {
        try
        {
            var users = _users.GetAll();
            gridUsers.DataSource = users.Select(u => new UserRow(
                u.Id, u.Username, u.Email,
                u.Role.ToString(),
                u.CreatedAt.ToLocalTime().ToString("dd-MM-yyyy"))).ToList();
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
        if (!TryGetId<PartRow>(gridParts, r => r.Id, out int id)) return;
        using var dlg = new PartEditForm(id);
        if (dlg.ShowDialog(this) == DialogResult.OK) LoadParts();
    }

    private void BtnDeletePart_Click(object sender, EventArgs e)
    {
        if (!TryGetId<PartRow>(gridParts, r => r.Id, out int id)) return;
        if (!Confirm("Delete this part?")) return;
        try   { _parts.Delete(id); LoadParts(); }
        catch (Exception ex) { ShowError(ex.Message); }
    }

    // ── Category CRUD ─────────────────────────────────────────────────
    private void BtnAddCat_Click(object sender, EventArgs e)
    {
        string name = Prompt("Category name:");
        if (string.IsNullOrWhiteSpace(name)) return;
        string desc = Prompt("Description (optional):");
        try   { _parts.AddCategory(name, desc); LoadCategories(); }
        catch (Exception ex) { ShowError(ex.Message); }
    }

    private void BtnDeleteCat_Click(object sender, EventArgs e)
    {
        if (!TryGetId<CategoryRow>(gridCats, r => r.Id, out int id)) return;
        if (!Confirm("Delete this category?")) return;
        try   { _parts.DeleteCategory(id); LoadCategories(); }
        catch (Exception ex) { ShowError($"Cannot delete: {ex.Message}"); }
    }

    // ── Manufacturer CRUD ─────────────────────────────────────────────
    private void BtnAddMfr_Click(object sender, EventArgs e)
    {
        string name = Prompt("Manufacturer name:");
        if (string.IsNullOrWhiteSpace(name)) return;
        string country = Prompt("Country:");
        string website = Prompt("Website URL:");
        try   { _parts.AddManufacturer(name, country, website); LoadManufacturers(); }
        catch (Exception ex) { ShowError(ex.Message); }
    }

    private void BtnDeleteMfr_Click(object sender, EventArgs e)
    {
        if (!TryGetId<ManufacturerRow>(gridMfr, r => r.Id, out int id)) return;
        if (!Confirm("Delete this manufacturer?")) return;
        try   { _parts.DeleteManufacturer(id); LoadManufacturers(); }
        catch (Exception ex) { ShowError($"Cannot delete: {ex.Message}"); }
    }

    // ── Users ─────────────────────────────────────────────────────────
    private void BtnToggleRole_Click(object sender, EventArgs e)
    {
        if (!TryGetId<UserRow>(gridUsers, r => r.Id, out int id)) return;
        if (id == Session.CurrentUser!.Id)
        { ShowInfo("You cannot change your own role."); return; }
        try   { _users.ToggleRole(id); LoadUsers(); }
        catch (Exception ex) { ShowError(ex.Message); }
    }

    // ── Helpers ──────────────────────────────────────────────────────

    private static void HideCol(DataGridView g, string col)
    {
        if (g.Columns.Contains(col)) g.Columns[col]!.Visible = false;
    }

    /// <summary>
    /// Type-safe row ID extraction — replaces the old dynamic cast pattern.
    /// Binds TRow to the record type that the grid is bound to.
    /// </summary>
    private static bool TryGetId<TRow>(
        DataGridView g, Func<TRow, int> idSelector, out int id)
    {
        id = 0;
        if (g.CurrentRow?.DataBoundItem is not TRow row) return false;
        id = idSelector(row);
        return true;
    }

    private static string Prompt(string question)
    {
        using var f = new Form
        {
            Width = 380, Height = 160, Text = question,
            StartPosition   = FormStartPosition.CenterParent,
            FormBorderStyle = FormBorderStyle.FixedDialog,
            MaximizeBox     = false,
        };
        var lbl   = new Label   { Text = question, AutoSize = true, Location = new Point(14, 16) };
        var txt   = new TextBox { Location = new Point(14, 38), Width = 340 };
        var btnOk = new Button  { Text = "OK",     DialogResult = DialogResult.OK,     Location = new Point(198, 76), Width = 70 };
        var btnNo = new Button  { Text = "Cancel", DialogResult = DialogResult.Cancel, Location = new Point(278, 76), Width = 70 };
        f.Controls.AddRange(new Control[] { lbl, txt, btnOk, btnNo });
        f.AcceptButton = btnOk;
        f.CancelButton = btnNo;
        return f.ShowDialog() == DialogResult.OK ? txt.Text : string.Empty;
    }
}
