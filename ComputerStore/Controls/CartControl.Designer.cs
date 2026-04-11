using ComputerStore.Infrastructure;

namespace ComputerStore
{
    partial class CartControl
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

            // ── Top bar ──────────────────────────────────────────────
            pnlTop = new Panel
            {
                Dock      = DockStyle.Top,
                Height    = 56,
                BackColor = AppColors.SurfaceWhite,
            };

            lblTitle          = UIFactory.HeaderLabel("🛒  Shopping Cart", 13);
            lblTitle.Location = new Point(12, 14);
            pnlTop.Controls.Add(lblTitle);

            // ── Grid ─────────────────────────────────────────────────
            grid = UIFactory.StyledGrid();
            // Track selection so RefreshGrid() can restore it (fix 2).
            grid.SelectionChanged += Grid_SelectionChanged;

            // ── Bottom bar ───────────────────────────────────────────
            // Uses two docked sub-panels so every button is always visible
            // regardless of window width (fix 3 — Place Order was off-screen).
            pnlBottom = new Panel
            {
                Dock      = DockStyle.Bottom,
                Height    = 58,
                BackColor = AppColors.SurfaceWhite,
            };

            // Left side: grand total label
            lblTotal = new Label
            {
                Text      = "Grand Total:  $0.00",
                Font      = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = AppColors.DarkBlue,
                AutoSize  = true,
                Anchor    = AnchorStyles.Left | AnchorStyles.Top,
                Location  = new Point(12, 18),
            };

            // Right side: action buttons in a docked flow panel
            pnlButtons = new FlowLayoutPanel
            {
                Dock          = DockStyle.Right,
                AutoSize      = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents  = false,
                BackColor     = AppColors.SurfaceWhite,
                Padding       = new Padding(0, 11, 8, 0),
            };

            btnDec        = UIFactory.SecondaryButton("－", 44, 34);
            btnDec.Click += BtnDec_Click;

            btnInc        = UIFactory.SecondaryButton("＋", 44, 34);
            btnInc.Click += BtnInc_Click;

            btnRemove        = UIFactory.SecondaryButton("Remove", 90, 34);
            btnRemove.Click += BtnRemove_Click;

            btnClear        = UIFactory.SecondaryButton("Clear All", 90, 34);
            btnClear.Click += BtnClear_Click;

            btnOrder        = UIFactory.PrimaryButton("✔ Place Order", 150, 34);
            btnOrder.Click += BtnOrder_Click;

            pnlButtons.Controls.Add(btnDec);
            pnlButtons.Controls.Add(btnInc);
            pnlButtons.Controls.Add(btnRemove);
            pnlButtons.Controls.Add(btnClear);
            pnlButtons.Controls.Add(btnOrder);

            pnlBottom.Controls.Add(lblTotal);
            pnlBottom.Controls.Add(pnlButtons);

            Controls.Add(grid);
            Controls.Add(pnlTop);
            Controls.Add(pnlBottom);
        }

        #endregion

        // ── Control declarations ─────────────────────────────────────
        private Panel           pnlTop     = null!;
        private Label           lblTitle   = null!;
        private DataGridView    grid       = null!;
        private Panel           pnlBottom  = null!;
        private FlowLayoutPanel pnlButtons = null!;
        private Label           lblTotal   = null!;
        private Button          btnDec     = null!;
        private Button          btnInc     = null!;
        private Button          btnRemove  = null!;
        private Button          btnClear   = null!;
        private Button          btnOrder   = null!;
    }
}
