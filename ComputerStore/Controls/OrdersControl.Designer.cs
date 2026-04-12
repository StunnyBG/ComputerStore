using ComputerStore.Infrastructure;

namespace ComputerStore
{
    partial class OrdersControl
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

            lblTitle          = UIFactory.HeaderLabel("📦  My Orders", 13);
            lblTitle.Location = new Point(12, 14);
            pnlTop.Controls.Add(lblTitle);

            // ── Bottom action bar ────────────────────────────────────
            pnlActions = new Panel
            {
                Dock      = DockStyle.Bottom,
                Height    = 50,
                BackColor = AppColors.SurfaceWhite,
            };

            lblStatus = new Label
            {
                Text      = string.Empty,
                AutoSize  = true,
                Font      = new Font("Segoe UI", 9f),
                ForeColor = AppColors.TextDark,
                Location  = new Point(12, 16),
            };

            btnCancelOrder          = UIFactory.SecondaryButton("✘ Cancel Order", 160, 34);
            btnCancelOrder.Location = new Point(630, 8);
            btnCancelOrder.ForeColor = AppColors.ErrorRed;
            btnCancelOrder.FlatAppearance.BorderColor = AppColors.ErrorRed;
            btnCancelOrder.Click   += BtnCancel_Click;

            pnlActions.Controls.Add(lblStatus);
            pnlActions.Controls.Add(btnCancelOrder);

            // ── Split: orders top / items bottom ─────────────────────
            split = new SplitContainer
            {
                Dock             = DockStyle.Fill,
                Orientation      = Orientation.Horizontal,
                SplitterDistance = 260,
                BackColor        = AppColors.Background,
            };

            // Orders panel
            lblOrders = new Label
            {
                Text      = "Orders",
                Font      = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = AppColors.PrimaryBlue,
                Dock      = DockStyle.Top,
                Height    = 26,
                Padding   = new Padding(4, 4, 0, 0),
            };

            gridOrders = UIFactory.StyledGrid();
            gridOrders.SelectionChanged += GridOrders_SelectionChanged;

            split.Panel1.Controls.Add(gridOrders);
            split.Panel1.Controls.Add(lblOrders);

            // Items panel
            lblItems = new Label
            {
                Text      = "Order Items",
                Font      = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = AppColors.PrimaryBlue,
                Dock      = DockStyle.Top,
                Height    = 26,
                Padding   = new Padding(4, 4, 0, 0),
            };

            gridItems = UIFactory.StyledGrid();

            split.Panel2.Controls.Add(gridItems);
            split.Panel2.Controls.Add(lblItems);

            Controls.Add(split);
            Controls.Add(pnlTop);
            Controls.Add(pnlActions);
        }

        #endregion

        // ── Control declarations ─────────────────────────────────────
        private Panel          pnlTop         = null!;
        private Label          lblTitle        = null!;
        private SplitContainer split           = null!;
        private DataGridView   gridOrders      = null!;
        private DataGridView   gridItems       = null!;
        private Label          lblOrders       = null!;
        private Label          lblItems        = null!;
        private Panel          pnlActions      = null!;
        private Button         btnCancelOrder  = null!;
        private Label          lblStatus       = null!;
    }
}
