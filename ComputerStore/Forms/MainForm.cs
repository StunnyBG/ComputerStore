// ══════════════════════════════════════════════════════════════════════
// OOP: MainForm inherits from Form (standard WinForms base class)
// DATA STRUCTURE: Stack<string> used for navigation history
// ══════════════════════════════════════════════════════════════════════
using ComputerStore.Infrastructure;   // BaseControl

namespace ComputerStore
{
    public partial class MainForm : Form    // INHERITANCE — Form is the WinForms base class
    {
        private Control? _currentView;

        // ── DATA STRUCTURE: Stack<string> — navigation history ───────────
        // REQUIREMENT: Stack is one of the ≥3 required data structures.
        // Stacks follow LIFO (Last In, First Out) — perfect for back-navigation
        // because the last page visited is the first one we return to.
        //
        // When the user navigates to a new view the previous view name is
        // pushed onto the stack. BtnBack pops and restores the previous view.
        private readonly Stack<string> _navHistory = new Stack<string>();

        public MainForm()
        {
            InitializeComponent();
            ApplySession();
            ShowCatalog();
        }

        // ── Navigation ────────────────────────────────────────────────────
        private void ShowCatalog() => LoadView(new CatalogControl(), "Catalog");
        private void ShowCart()    => LoadView(new CartControl(),    "Cart");
        private void ShowOrders()  => LoadView(new OrdersControl(),  "Orders");
        private void ShowAdmin()   => LoadView(new AdminControl(),   "Admin");

        private void LoadView(Control view, string viewName)
        {
            if (_currentView is not null)
            {
                // Push current view name onto the Stack before leaving it
                // Stack.Push() — O(1) LIFO insertion
                if (_currentView.Tag is string currentName)
                    _navHistory.Push(currentName);

                pnlContent.Controls.Remove(_currentView);
                _currentView.Dispose();
            }

            view.Dock = DockStyle.Fill;
            view.Tag  = viewName;               // store name so we can push it later
            pnlContent.Controls.Add(view);
            _currentView = view;

            // Enable the back button only when there is history to return to
            btnBack.Enabled = _navHistory.Count > 0;
        }

        private void ApplySession()
        {
            lblUserInfo.Text = $"👤 {Session.CurrentUser?.Username}\n({Session.CurrentUser?.Role})";
            btnAdmin.Visible = Session.IsAdmin;
        }

        // ── Event handlers ────────────────────────────────────────────────
        private void BtnCatalog_Click(object sender, EventArgs e) => ShowCatalog();
        private void BtnCart_Click(object sender, EventArgs e)    => ShowCart();
        private void BtnOrders_Click(object sender, EventArgs e)  => ShowOrders();
        private void BtnAdmin_Click(object sender, EventArgs e)   => ShowAdmin();

        private void BtnBack_Click(object sender, EventArgs e)
        {
            // Stack.Pop() — O(1) LIFO removal: returns the last pushed view name
            if (_navHistory.Count == 0) return;
            string previous = _navHistory.Pop();

            // Navigate to the previous view without pushing again
            // (clear current first so LoadView does not push an extra entry)
            if (_currentView is not null)
            {
                pnlContent.Controls.Remove(_currentView);
                _currentView.Dispose();
                _currentView = null;
            }

            Control view = previous switch
            {
                "Catalog" => new CatalogControl(),
                "Cart"    => new CartControl(),
                "Orders"  => new OrdersControl(),
                "Admin"   => new AdminControl(),
                _         => new CatalogControl()
            };
            view.Dock = DockStyle.Fill;
            view.Tag  = previous;
            pnlContent.Controls.Add(view);
            _currentView    = view;
            btnBack.Enabled = _navHistory.Count > 0;
        }

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
