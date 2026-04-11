using ComputerStore.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ComputerStore
{
    public partial class PartEditForm : Form
    {
        private readonly int _partId; // 0 = new part

        public PartEditForm(int partId = 0)
        {
            _partId = partId;
            InitializeComponent();
            LoadDropdowns();
            if (_partId != 0) LoadExistingPart();
        }

        // ── Data loading ──────────────────────────────────────────────
        private void LoadDropdowns()
        {
            try
            {
                using var ctx = DbContextFactory.Create();
                cmbCategory.DataSource    = ctx.Categories.AsNoTracking().OrderBy(c => c.Name).ToList();
                cmbCategory.DisplayMember = "Name";
                cmbCategory.ValueMember   = "Id";

                cmbManufacturer.DataSource    = ctx.Manufacturers.AsNoTracking().OrderBy(m => m.Name).ToList();
                cmbManufacturer.DisplayMember = "Name";
                cmbManufacturer.ValueMember   = "Id";
            }
            catch (Exception ex) { lblError.Text = ex.Message; }
        }

        private void LoadExistingPart()
        {
            try
            {
                using var ctx = DbContextFactory.Create();
                var p = ctx.PcParts.AsNoTracking().First(x => x.Id == _partId);

                txtName.Text    = p.Name;
                txtDesc.Text    = p.Description ?? string.Empty;
                txtPrice.Text   = p.Price.ToString("F2");
                txtStock.Text   = p.Stock.ToString();
                txtImgPath.Text = p.ImagePath ?? string.Empty;

                SelectComboById(cmbCategory,     p.CategoryId);
                SelectComboById(cmbManufacturer, p.ManufacturerId);
            }
            catch (Exception ex) { lblError.Text = ex.Message; }
        }

        private static void SelectComboById(ComboBox cmb, int id)
        {
            foreach (var item in cmb.Items)
            {
                dynamic d = item!;
                if ((int)d.Id == id) { cmb.SelectedItem = item; return; }
            }
        }

        // ── Save ──────────────────────────────────────────────────────
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
            { lblError.Text = "Price must be between 0.01 and 999999.99."; return; }

            if (!int.TryParse(txtStock.Text, out int stock) || stock < 0)
            { lblError.Text = "Stock must be a non-negative whole number."; return; }

            if (cmbCategory.SelectedItem is null)     { lblError.Text = "Select a category.";     return; }
            if (cmbManufacturer.SelectedItem is null) { lblError.Text = "Select a manufacturer."; return; }

            dynamic cat = cmbCategory.SelectedItem!;
            dynamic mfr = cmbManufacturer.SelectedItem!;
            int catId = (int)cat.Id;
            int mfrId = (int)mfr.Id;

            string? imagePath = txtImgPath.Text.Trim().Length > 0 ? txtImgPath.Text.Trim() : null;

            try
            {
                using var ctx = DbContextFactory.Create();

                if (_partId == 0)
                {
                    ctx.PcParts.Add(new PcPart
                    {
                        Name           = name,
                        Description    = txtDesc.Text.Trim(),
                        Price          = price,
                        Stock          = stock,
                        ImagePath      = imagePath,
                        CategoryId     = catId,
                        ManufacturerId = mfrId,
                        CreatedAt      = DateTime.UtcNow,
                    });
                }
                else
                {
                    var p = ctx.PcParts.First(x => x.Id == _partId);
                    p.Name           = name;
                    p.Description    = txtDesc.Text.Trim();
                    p.Price          = price;
                    p.Stock          = stock;
                    p.ImagePath      = imagePath;
                    p.CategoryId     = catId;
                    p.ManufacturerId = mfrId;
                }

                ctx.SaveChanges();
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex) { lblError.Text = $"Error: {ex.Message}"; }
        }

        private void BtnCancel_Click(object sender, EventArgs e) => Close();
    }
}
