using ComputerStore.Infrastructure;

namespace ComputerStore
{
    partial class PartEditForm
    {
        private System.ComponentModel.IContainer components = null!;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            // ── Form ────────────────────────────────────────────────
            Text            = _partId == 0 ? "Add PC Part" : "Edit PC Part";
            Size            = new Size(500, 620);
            StartPosition   = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox     = false;
            BackColor       = AppColors.Background;
            Font            = new Font("Segoe UI", 9.5f);

            // ── Header ───────────────────────────────────────────────
            pnlHeader = new Panel
            {
                Dock      = DockStyle.Top,
                Height    = 70,
                BackColor = AppColors.PrimaryBlue,
            };

            lblTitle = new Label
            {
                Text      = _partId == 0 ? "➕  Add New PC Part" : "✎  Edit PC Part",
                Font      = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize  = true,
                Location  = new Point(16, 18),
            };
            pnlHeader.Controls.Add(lblTitle);

            // ── Card ─────────────────────────────────────────────────
            pnlCard = new Panel
            {
                Size      = new Size(448, 490),
                Location  = new Point(24, 86),
                BackColor = AppColors.SurfaceWhite,
            };
            pnlCard.Paint += (s, e) => e.Graphics.DrawRectangle(
                new Pen(AppColors.BorderGray), 0, 0, pnlCard.Width - 1, pnlCard.Height - 1);

            // ── Rows ─────────────────────────────────────────────────
            // Name
            lblName          = UIFactory.FieldLabel("Name *");
            lblName.Location = new Point(16, 18);
            txtName          = UIFactory.StyledTextBox(408);
            txtName.Location = new Point(16, 38);

            // Description
            lblDesc          = UIFactory.FieldLabel("Description");
            lblDesc.Location = new Point(16, 76);
            txtDesc = new TextBox
            {
                Location    = new Point(16, 96),
                Width       = 408,
                Height      = 60,
                Multiline   = true,
                ScrollBars  = ScrollBars.Vertical,
                Font        = new Font("Segoe UI", 10f),
                BorderStyle = BorderStyle.FixedSingle,
            };

            // Price
            lblPrice          = UIFactory.FieldLabel("Price ($) *");
            lblPrice.Location = new Point(16, 172);
            txtPrice          = UIFactory.StyledTextBox(180);
            txtPrice.Location = new Point(16, 192);

            // Stock
            lblStock          = UIFactory.FieldLabel("Stock *");
            lblStock.Location = new Point(216, 172);
            txtStock          = UIFactory.StyledTextBox(140);
            txtStock.Location = new Point(216, 192);

            // Category
            lblCategory          = UIFactory.FieldLabel("Category *");
            lblCategory.Location = new Point(16, 232);
            cmbCategory = new ComboBox
            {
                Location      = new Point(16, 252),
                Width         = 200,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font          = new Font("Segoe UI", 10f),
            };

            // Manufacturer
            lblManufacturer          = UIFactory.FieldLabel("Manufacturer *");
            lblManufacturer.Location = new Point(232, 232);
            cmbManufacturer = new ComboBox
            {
                Location      = new Point(232, 252),
                Width         = 200,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font          = new Font("Segoe UI", 10f),
            };

            // Image path
            lblImgPath          = UIFactory.FieldLabel("Image Path (optional)");
            lblImgPath.Location = new Point(16, 292);
            txtImgPath          = UIFactory.StyledTextBox(408);
            txtImgPath.Location = new Point(16, 312);

            // Error
            lblError = new Label
            {
                Text      = string.Empty,
                Location  = new Point(16, 352),
                Size      = new Size(412, 40),
                Font      = new Font("Segoe UI", 9f, FontStyle.Italic),
                ForeColor = AppColors.ErrorRed,
            };

            // Buttons
            btnSave          = UIFactory.PrimaryButton("💾 Save", 130, 40);
            btnSave.Location = new Point(16, 402);
            btnSave.Click   += BtnSave_Click;

            btnCancel          = UIFactory.SecondaryButton("Cancel", 100, 40);
            btnCancel.Location = new Point(158, 402);
            btnCancel.Click   += BtnCancel_Click;

            pnlCard.Controls.Add(lblName);
            pnlCard.Controls.Add(txtName);
            pnlCard.Controls.Add(lblDesc);
            pnlCard.Controls.Add(txtDesc);
            pnlCard.Controls.Add(lblPrice);
            pnlCard.Controls.Add(txtPrice);
            pnlCard.Controls.Add(lblStock);
            pnlCard.Controls.Add(txtStock);
            pnlCard.Controls.Add(lblCategory);
            pnlCard.Controls.Add(cmbCategory);
            pnlCard.Controls.Add(lblManufacturer);
            pnlCard.Controls.Add(cmbManufacturer);
            pnlCard.Controls.Add(lblImgPath);
            pnlCard.Controls.Add(txtImgPath);
            pnlCard.Controls.Add(lblError);
            pnlCard.Controls.Add(btnSave);
            pnlCard.Controls.Add(btnCancel);

            Controls.Add(pnlHeader);
            Controls.Add(pnlCard);
        }

        #endregion

        // ── Control declarations ─────────────────────────────────────
        private Panel    pnlHeader       = null!;
        private Label    lblTitle        = null!;
        private Panel    pnlCard         = null!;
        private Label    lblName         = null!;
        private TextBox  txtName         = null!;
        private Label    lblDesc         = null!;
        private TextBox  txtDesc         = null!;
        private Label    lblPrice        = null!;
        private TextBox  txtPrice        = null!;
        private Label    lblStock        = null!;
        private TextBox  txtStock        = null!;
        private Label    lblCategory     = null!;
        private ComboBox cmbCategory     = null!;
        private Label    lblManufacturer = null!;
        private ComboBox cmbManufacturer = null!;
        private Label    lblImgPath      = null!;
        private TextBox  txtImgPath      = null!;
        private Label    lblError        = null!;
        private Button   btnSave         = null!;
        private Button   btnCancel       = null!;
    }
}
