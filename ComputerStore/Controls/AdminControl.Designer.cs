using ComputerStore.Infrastructure;

namespace ComputerStore
{
    partial class AdminControl
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

            lblTitle          = UIFactory.HeaderLabel("⚙️  Admin Panel", 13);
            lblTitle.Location = new Point(12, 14);
            pnlTop.Controls.Add(lblTitle);

            // ── Tab control ──────────────────────────────────────────
            tabs = new TabControl
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10f),
            };

            // ════════════════════════════════════════════════════════
            // TAB 1 – PC Parts
            // ════════════════════════════════════════════════════════
            tabParts = new TabPage("🖥️  PC Parts")
            {
                BackColor = AppColors.Background,
            };

            gridParts = UIFactory.StyledGrid();

            pnlPartsBar = new Panel
            {
                Dock      = DockStyle.Bottom,
                Height    = 50,
                BackColor = AppColors.SurfaceWhite,
            };

            lblPartsStatus = new Label
            {
                Text      = string.Empty,
                AutoSize  = true,
                Font      = new Font("Segoe UI", 9f),
                ForeColor = AppColors.TextDark,
                Location  = new Point(12, 16),
            };

            btnAddPart          = UIFactory.PrimaryButton("➕ Add", 90, 34);
            btnAddPart.Location = new Point(580, 8);
            btnAddPart.Click   += BtnAddPart_Click;

            btnEditPart          = UIFactory.SecondaryButton("✎ Edit", 80, 34);
            btnEditPart.Location = new Point(678, 8);
            btnEditPart.Click   += BtnEditPart_Click;

            btnDeletePart          = UIFactory.SecondaryButton("✘ Delete", 90, 34);
            btnDeletePart.Location = new Point(766, 8);
            btnDeletePart.ForeColor = AppColors.ErrorRed;
            btnDeletePart.FlatAppearance.BorderColor = AppColors.ErrorRed;
            btnDeletePart.Click   += BtnDeletePart_Click;

            pnlPartsBar.Controls.Add(lblPartsStatus);
            pnlPartsBar.Controls.Add(btnAddPart);
            pnlPartsBar.Controls.Add(btnEditPart);
            pnlPartsBar.Controls.Add(btnDeletePart);

            tabParts.Controls.Add(gridParts);
            tabParts.Controls.Add(pnlPartsBar);

            // ════════════════════════════════════════════════════════
            // TAB 2 – Categories
            // ════════════════════════════════════════════════════════
            tabCats = new TabPage("📂  Categories")
            {
                BackColor = AppColors.Background,
            };

            gridCats = UIFactory.StyledGrid();

            pnlCatsBar = new Panel
            {
                Dock      = DockStyle.Bottom,
                Height    = 50,
                BackColor = AppColors.SurfaceWhite,
            };

            lblCatStatus = new Label
            {
                Text      = string.Empty,
                AutoSize  = true,
                Font      = new Font("Segoe UI", 9f),
                ForeColor = AppColors.TextDark,
                Location  = new Point(12, 16),
            };

            btnAddCat          = UIFactory.PrimaryButton("➕ Add", 90, 34);
            btnAddCat.Location = new Point(678, 8);
            btnAddCat.Click   += BtnAddCat_Click;

            btnDeleteCat          = UIFactory.SecondaryButton("✘ Delete", 90, 34);
            btnDeleteCat.Location = new Point(776, 8);
            btnDeleteCat.ForeColor = AppColors.ErrorRed;
            btnDeleteCat.FlatAppearance.BorderColor = AppColors.ErrorRed;
            btnDeleteCat.Click   += BtnDeleteCat_Click;

            pnlCatsBar.Controls.Add(lblCatStatus);
            pnlCatsBar.Controls.Add(btnAddCat);
            pnlCatsBar.Controls.Add(btnDeleteCat);

            tabCats.Controls.Add(gridCats);
            tabCats.Controls.Add(pnlCatsBar);

            // ════════════════════════════════════════════════════════
            // TAB 3 – Manufacturers
            // ════════════════════════════════════════════════════════
            tabMfr = new TabPage("🏭  Manufacturers")
            {
                BackColor = AppColors.Background,
            };

            gridMfr = UIFactory.StyledGrid();

            pnlMfrBar = new Panel
            {
                Dock      = DockStyle.Bottom,
                Height    = 50,
                BackColor = AppColors.SurfaceWhite,
            };

            lblMfrStatus = new Label
            {
                Text      = string.Empty,
                AutoSize  = true,
                Font      = new Font("Segoe UI", 9f),
                ForeColor = AppColors.TextDark,
                Location  = new Point(12, 16),
            };

            btnAddMfr          = UIFactory.PrimaryButton("➕ Add", 90, 34);
            btnAddMfr.Location = new Point(678, 8);
            btnAddMfr.Click   += BtnAddMfr_Click;

            btnDeleteMfr          = UIFactory.SecondaryButton("✘ Delete", 90, 34);
            btnDeleteMfr.Location = new Point(776, 8);
            btnDeleteMfr.ForeColor = AppColors.ErrorRed;
            btnDeleteMfr.FlatAppearance.BorderColor = AppColors.ErrorRed;
            btnDeleteMfr.Click   += BtnDeleteMfr_Click;

            pnlMfrBar.Controls.Add(lblMfrStatus);
            pnlMfrBar.Controls.Add(btnAddMfr);
            pnlMfrBar.Controls.Add(btnDeleteMfr);

            tabMfr.Controls.Add(gridMfr);
            tabMfr.Controls.Add(pnlMfrBar);

            // ════════════════════════════════════════════════════════
            // TAB 4 – Users
            // ════════════════════════════════════════════════════════
            tabUsers = new TabPage("👥  Users")
            {
                BackColor = AppColors.Background,
            };

            gridUsers = UIFactory.StyledGrid();

            pnlUsersBar = new Panel
            {
                Dock      = DockStyle.Bottom,
                Height    = 50,
                BackColor = AppColors.SurfaceWhite,
            };

            lblUsersStatus = new Label
            {
                Text      = string.Empty,
                AutoSize  = true,
                Font      = new Font("Segoe UI", 9f),
                ForeColor = AppColors.TextDark,
                Location  = new Point(12, 16),
            };

            btnToggleRole          = UIFactory.SecondaryButton("⇄ Toggle Role", 140, 34);
            btnToggleRole.Location = new Point(728, 8);
            btnToggleRole.Click   += BtnToggleRole_Click;

            pnlUsersBar.Controls.Add(lblUsersStatus);
            pnlUsersBar.Controls.Add(btnToggleRole);

            tabUsers.Controls.Add(gridUsers);
            tabUsers.Controls.Add(pnlUsersBar);

            // ── Assemble tabs ────────────────────────────────────────
            tabs.TabPages.Add(tabParts);
            tabs.TabPages.Add(tabCats);
            tabs.TabPages.Add(tabMfr);
            tabs.TabPages.Add(tabUsers);

            Controls.Add(tabs);
            Controls.Add(pnlTop);
        }

        #endregion

        // ── Control declarations ─────────────────────────────────────

        // Top bar
        private Panel    pnlTop  = null!;
        private Label    lblTitle = null!;

        // Tab control
        private TabControl tabs    = null!;
        private TabPage    tabParts = null!;
        private TabPage    tabCats  = null!;
        private TabPage    tabMfr   = null!;
        private TabPage    tabUsers = null!;

        // PC Parts tab
        private DataGridView gridParts      = null!;
        private Panel        pnlPartsBar    = null!;
        private Label        lblPartsStatus = null!;
        private Button       btnAddPart     = null!;
        private Button       btnEditPart    = null!;
        private Button       btnDeletePart  = null!;

        // Categories tab
        private DataGridView gridCats    = null!;
        private Panel        pnlCatsBar  = null!;
        private Label        lblCatStatus = null!;
        private Button       btnAddCat   = null!;
        private Button       btnDeleteCat = null!;

        // Manufacturers tab
        private DataGridView gridMfr     = null!;
        private Panel        pnlMfrBar   = null!;
        private Label        lblMfrStatus = null!;
        private Button       btnAddMfr   = null!;
        private Button       btnDeleteMfr = null!;

        // Users tab
        private DataGridView gridUsers      = null!;
        private Panel        pnlUsersBar    = null!;
        private Label        lblUsersStatus = null!;
        private Button       btnToggleRole  = null!;
    }
}
