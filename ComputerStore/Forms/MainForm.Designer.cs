using ComputerStore.Infrastructure;

namespace ComputerStore
{
    partial class MainForm
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
            Text          = "💻 ComputerStore";
            Size          = new Size(1050, 680);
            MinimumSize   = new Size(900, 580);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor     = AppColors.Background;
            Font          = new Font("Segoe UI", 9.5f);

            // ── Sidebar ──────────────────────────────────────────────
            pnlSidebar = new Panel
            {
                Dock      = DockStyle.Left,
                Width     = 200,
                BackColor = AppColors.PrimaryBlue,
            };

            lblLogo = new Label
            {
                Text      = "💻 ComputerStore",
                Font      = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = AppColors.AccentYellow,
                Dock      = DockStyle.Top,
                Height    = 58,
                TextAlign = ContentAlignment.MiddleCenter,
            };

            // Nav buttons — Dock=Top so add in bottom-to-top order
            btnAdmin = CreateSidebarButton("⚙️  Admin Panel");
            btnAdmin.Click += BtnAdmin_Click;

            btnOrders = CreateSidebarButton("📦  My Orders");
            btnOrders.Click += BtnOrders_Click;

            btnCart = CreateSidebarButton("🛒  Cart");
            btnCart.Click += BtnCart_Click;

            btnCatalog = CreateSidebarButton("🖥️  Catalog");
            btnCatalog.Click += BtnCatalog_Click;

            // Back button — uses the navigation Stack (DATA STRUCTURE)
            btnBack = CreateSidebarButton("◀  Back");
            btnBack.Click   += BtnBack_Click;
            btnBack.Enabled  = false;          // disabled until there is history
            btnBack.ForeColor = Color.FromArgb(180, 210, 255);

            // Divider
            pnlDivider = new Panel
            {
                Dock      = DockStyle.Bottom,
                Height    = 1,
                BackColor = AppColors.LightBlue,
            };

            // Logout at very bottom
            btnLogout = CreateSidebarButton("🚪  Logout");
            btnLogout.Dock      = DockStyle.Bottom;
            btnLogout.BackColor = AppColors.DarkBlue;
            btnLogout.Click    += BtnLogout_Click;

            lblUserInfo = new Label
            {
                Text      = string.Empty,
                Font      = new Font("Segoe UI", 8.5f),
                ForeColor = Color.FromArgb(180, 210, 255),
                Dock      = DockStyle.Bottom,
                Height    = 50,
                TextAlign = ContentAlignment.MiddleCenter,
            };

            // Add controls — Dock=Top items rendered top-to-bottom
            pnlSidebar.Controls.Add(btnAdmin);
            pnlSidebar.Controls.Add(btnOrders);
            pnlSidebar.Controls.Add(btnCart);
            pnlSidebar.Controls.Add(btnCatalog);
            pnlSidebar.Controls.Add(btnBack);
            pnlSidebar.Controls.Add(lblLogo);
            pnlSidebar.Controls.Add(pnlDivider);
            pnlSidebar.Controls.Add(btnLogout);
            pnlSidebar.Controls.Add(lblUserInfo);

            // ── Content area ─────────────────────────────────────────
            pnlContent = new Panel
            {
                Dock      = DockStyle.Fill,
                BackColor = AppColors.Background,
            };

            Controls.Add(pnlContent);
            Controls.Add(pnlSidebar);
        }

        #endregion

        // ── Helper: consistent sidebar button ────────────────────────
        private static Button CreateSidebarButton(string text)
        {
            var btn = new Button
            {
                Text      = text,
                Dock      = DockStyle.Top,
                Height    = 48,
                FlatStyle = FlatStyle.Flat,
                BackColor = AppColors.PrimaryBlue,
                ForeColor = Color.White,
                Font      = new Font("Segoe UI", 10.5f),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding   = new Padding(16, 0, 0, 0),
                Cursor    = Cursors.Hand,
            };
            btn.FlatAppearance.BorderSize         = 0;
            btn.FlatAppearance.MouseOverBackColor = AppColors.LightBlue;
            return btn;
        }

        // ── Control declarations ─────────────────────────────────────
        private Panel  pnlSidebar  = null!;
        private Panel  pnlContent  = null!;
        private Label  lblLogo     = null!;
        private Label  lblUserInfo = null!;
        private Button btnCatalog  = null!;
        private Button btnCart     = null!;
        private Button btnOrders   = null!;
        private Button btnAdmin    = null!;
        private Button btnBack     = null!;   // Stack-powered back navigation
        private Button btnLogout   = null!;
        private Panel  pnlDivider  = null!;
    }
}
