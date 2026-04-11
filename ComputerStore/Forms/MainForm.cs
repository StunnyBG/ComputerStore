namespace ComputerStore
{
    public partial class MainForm : Form
    {
        private Control? _currentView;

        public MainForm()
        {
            InitializeComponent();
            ApplySession();
            ShowCatalog();
        }

        // ── Navigation ────────────────────────────────────────────────
        private void ShowCatalog() => LoadView(new CatalogControl());
        private void ShowCart()    => LoadView(new CartControl());
        private void ShowOrders()  => LoadView(new OrdersControl());
        private void ShowAdmin()   => LoadView(new AdminControl());

        private void LoadView(Control view)
        {
            if (_currentView is not null)
            {
                pnlContent.Controls.Remove(_currentView);
                _currentView.Dispose();
            }
            view.Dock = DockStyle.Fill;
            pnlContent.Controls.Add(view);
            _currentView = view;
        }

        private void ApplySession()
        {
            lblUserInfo.Text     = $"👤 {Session.CurrentUser?.Username}\n({Session.CurrentUser?.Role})";
            btnAdmin.Visible     = Session.IsAdmin;
        }

        // ── Event handlers ────────────────────────────────────────────
        private void BtnCatalog_Click(object sender, EventArgs e) => ShowCatalog();
        private void BtnCart_Click(object sender, EventArgs e)    => ShowCart();
        private void BtnOrders_Click(object sender, EventArgs e)  => ShowOrders();
        private void BtnAdmin_Click(object sender, EventArgs e)   => ShowAdmin();

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Log out?", "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Session.Logout();
                Application.Restart();
            }
        }
    }
}
