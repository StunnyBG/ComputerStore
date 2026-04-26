using ComputerStore.Data.Models;
using ComputerStore.Infrastructure;

namespace ComputerStore;

public partial class PartEditForm : Form
{
    private readonly int          _partId;
    private readonly IPartService _parts = ServiceLocator.Parts;

    public PartEditForm(int partId = 0)
    {
        _partId = partId;
        InitializeComponent();
        LoadDropdowns();
        if (_partId != 0) LoadExistingPart();
    }

    // ── Data loading ──────────────────────────────────────────────────
    private void LoadDropdowns()
    {
        try
        {
            cmbCategory.DataSource    = _parts.GetCategories();
            cmbCategory.DisplayMember = "Name";
            cmbCategory.ValueMember   = "Id";

            cmbManufacturer.DataSource    = _parts.GetManufacturers();
            cmbManufacturer.DisplayMember = "Name";
            cmbManufacturer.ValueMember   = "Id";
        }
        catch (Exception ex) { lblError.Text = ex.Message; }
    }

    private void LoadExistingPart()
    {
        try
        {
            var p = _parts.GetById(_partId);
            if (p is null) { lblError.Text = "Part not found."; return; }

            txtName.Text    = p.Name;
            txtDesc.Text    = p.Description ?? string.Empty;
            txtPrice.Text   = p.Price.ToString("F2");
            txtStock.Text   = p.Stock.ToString();
            txtImgPath.Text = p.ImagePath ?? string.Empty;

            // FIX: generic SelectById<T> instead of dynamic cast
            SelectById<Category>    (cmbCategory,     p.CategoryId,     c => c.Id);
            SelectById<Manufacturer>(cmbManufacturer, p.ManufacturerId, m => m.Id);
        }
        catch (Exception ex) { lblError.Text = ex.Message; }
    }

    /// <summary>
    /// Selects the ComboBox item whose Id matches <paramref name="id"/>.
    /// Fully type-safe — no dynamic, no RuntimeBinderException.
    /// </summary>
    private static void SelectById<T>(ComboBox cmb, int id, Func<T, int> idSelector)
    {
        foreach (var item in cmb.Items.OfType<T>())
            if (idSelector(item) == id) { cmb.SelectedItem = item; return; }
    }

    // ── Save ──────────────────────────────────────────────────────────
    private void BtnSave_Click(object sender, EventArgs e)
    {
        lblError.Text = string.Empty;

        string name = txtName.Text.Trim();
        if (name.Length < 2 || name.Length > 100)
        { lblError.Text = "Name must be 2–100 characters."; return; }

        if (!decimal.TryParse(txtPrice.Text.Replace(',', '.'),
                System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture,
                out decimal price)
            || price < 0.01m || price > 999999.99m)
        { lblError.Text = "Price must be between 0.01 and 999 999.99."; return; }

        if (!int.TryParse(txtStock.Text, out int stock) || stock < 0)
        { lblError.Text = "Stock must be a non-negative whole number."; return; }

        if (cmbCategory.SelectedItem     is not Category     cat)
        { lblError.Text = "Select a category."; return; }

        if (cmbManufacturer.SelectedItem is not Manufacturer mfr)
        { lblError.Text = "Select a manufacturer."; return; }

        string? imagePath = txtImgPath.Text.Trim() is { Length: > 0 } s ? s : null;

        try
        {
            var part = new PcPart
            {
                Id             = _partId,
                Name           = name,
                Description    = txtDesc.Text.Trim(),
                Price          = price,
                Stock          = stock,
                ImagePath      = imagePath,
                CategoryId     = cat.Id,
                ManufacturerId = mfr.Id,
                CreatedAt      = DateTime.UtcNow,
            };

            if (_partId == 0) _parts.Add(part);
            else              _parts.Update(part);

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex) { lblError.Text = $"Error: {ex.Message}"; }
    }

    private void BtnCancel_Click(object sender, EventArgs e) => Close();
}
