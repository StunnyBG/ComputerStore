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

            // ── Bottom bar ───────────────────────────────────────────
            pnlBottom = new Panel
            {
                Dock      = DockStyle.Bottom,
                Height    = 58,
                BackColor = AppColors.SurfaceWhite,
                Padding   = new Padding(12, 10, 12, 10),
            };

            lblTotal = new Label
            {
                Text      = "Grand Total:  $0.00",
                Font      = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = AppColors.DarkBlue,
                AutoSize  = true,
                Location  = new Point(12, 18),
            };

            btnDec          = UIFactory.SecondaryButton("－", 44, 34);
            btnDec.Location = new Point(310, 12);
            btnDec.Click   += BtnDec_Click;

            btnInc          = UIFactory.SecondaryButton("＋", 44, 34);
            btnInc.Location = new Point(360, 12);
            btnInc.Click   += BtnInc_Click;

            btnRemove          = UIFactory.SecondaryButton("Remove", 90, 34);
            btnRemove.Location = new Point(412, 12);
            btnRemove.Click   += BtnRemove_Click;

            btnClear          = UIFactory.SecondaryButton("Clear All", 90, 34);
            btnClear.Location = new Point(510, 12);
            btnClear.Click   += BtnClear_Click;

            btnOrder          = UIFactory.PrimaryButton("✔ Place Order", 150, 34);
            btnOrder.Anchor   = AnchorStyles.Top | AnchorStyles.Right;
            btnOrder.Location = new Point(720, 12);
            btnOrder.Click   += BtnOrder_Click;

            pnlBottom.Controls.Add(lblTotal);
            pnlBottom.Controls.Add(btnDec);
            pnlBottom.Controls.Add(btnInc);
            pnlBottom.Controls.Add(btnRemove);
            pnlBottom.Controls.Add(btnClear);
            pnlBottom.Controls.Add(btnOrder);

            Controls.Add(grid);
            Controls.Add(pnlTop);
            Controls.Add(pnlBottom);
        }

        #endregion

        // ── Control declarations ─────────────────────────────────────
        private Panel        pnlTop    = null!;
        private Label        lblTitle  = null!;
        private DataGridView grid      = null!;
        private Panel        pnlBottom = null!;
        private Label        lblTotal  = null!;
        private Button       btnDec    = null!;
        private Button       btnInc    = null!;
        private Button       btnRemove = null!;
        private Button       btnClear  = null!;
        private Button       btnOrder  = null!;
    }
}
