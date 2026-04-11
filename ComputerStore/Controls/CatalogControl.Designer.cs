using ComputerStore.Infrastructure;

namespace ComputerStore
{
    partial class CatalogControl
    {
        private System.ComponentModel.IContainer components = null!;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            Dock      = DockStyle.Fill;
            BackColor = AppColors.Background;

            // ── Top toolbar ──────────────────────────────────────────
            pnlTop = new Panel
            {
                Dock      = DockStyle.Top,
                Height    = 60,
                BackColor = AppColors.SurfaceWhite,
                Padding   = new Padding(12, 10, 12, 10),
            };
            pnlTop.Paint += (s, e) => e.Graphics.DrawLine(
                new Pen(AppColors.BorderGray), 0, pnlTop.Height - 1, pnlTop.Width, pnlTop.Height - 1);

            lblTitle          = UIFactory.HeaderLabel("🖥️  Parts Catalog", 13);
            lblTitle.Location = new Point(12, 16);

            txtSearch = new TextBox
            {
                Width           = 220,
                Height          = 28,
                Font            = new Font("Segoe UI", 10f),
                BorderStyle     = BorderStyle.FixedSingle,
                Location        = new Point(210, 15),
                PlaceholderText = "Search parts…",
            };
            txtSearch.KeyDown += TxtSearch_KeyDown;

            cmbCategory = new ComboBox
            {
                Width         = 160,
                Height        = 28,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font          = new Font("Segoe UI", 10f),
                Location      = new Point(440, 15),
            };

            btnSearch          = UIFactory.PrimaryButton("🔍 Search", 110, 32);
            btnSearch.Location = new Point(610, 14);
            btnSearch.Click   += BtnSearch_Click;

            btnClear          = UIFactory.SecondaryButton("Clear", 70, 32);
            btnClear.Location = new Point(728, 14);
            btnClear.Click   += BtnClear_Click;

            pnlTop.Controls.Add(lblTitle);
            pnlTop.Controls.Add(txtSearch);
            pnlTop.Controls.Add(cmbCategory);
            pnlTop.Controls.Add(btnSearch);
            pnlTop.Controls.Add(btnClear);

            // ── Grid ─────────────────────────────────────────────────
            grid = UIFactory.StyledGrid();

            // ── Bottom action bar ────────────────────────────────────
            pnlBottom = new Panel
            {
                Dock      = DockStyle.Bottom,
                Height    = 50,
                BackColor = AppColors.SurfaceWhite,
                Padding   = new Padding(12, 8, 12, 8),
            };

            lblStatus = new Label
            {
                Text      = string.Empty,
                Font      = new Font("Segoe UI", 9f),
                ForeColor = AppColors.TextDark,
                AutoSize  = true,
                Location  = new Point(12, 15),
            };

            btnDetails          = UIFactory.SecondaryButton("ℹ️ Details", 110, 34);
            btnDetails.Anchor   = AnchorStyles.Top | AnchorStyles.Right;
            btnDetails.Location = new Point(598, 8);
            btnDetails.Click   += BtnDetails_Click;

            btnAddCart          = UIFactory.PrimaryButton("🛒 Add to Cart", 140, 34);
            btnAddCart.Anchor   = AnchorStyles.Top | AnchorStyles.Right;
            btnAddCart.Location = new Point(718, 8);
            btnAddCart.Click   += BtnAddCart_Click;

            pnlBottom.Controls.Add(lblStatus);
            pnlBottom.Controls.Add(btnDetails);
            pnlBottom.Controls.Add(btnAddCart);

            Controls.Add(grid);
            Controls.Add(pnlTop);
            Controls.Add(pnlBottom);
        }

        #endregion

        // ── Control declarations ─────────────────────────────────────
        private Panel        pnlTop      = null!;
        private Label        lblTitle    = null!;
        private TextBox      txtSearch   = null!;
        private ComboBox     cmbCategory = null!;
        private Button       btnSearch   = null!;
        private Button       btnClear    = null!;
        private DataGridView grid        = null!;
        private Panel        pnlBottom   = null!;
        private Label        lblStatus   = null!;
        private Button       btnAddCart  = null!;
        private Button       btnDetails  = null!;
    }
}
