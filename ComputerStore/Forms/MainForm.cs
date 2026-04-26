// ══════════════════════════════════════════════════════════════════════
// DATA STRUCTURE: Stack<ViewName> — LIFO navigation history
// OOP:           MainForm inherits from Form
// ══════════════════════════════════════════════════════════════════════
using ComputerStore.Infrastructure;

namespace ComputerStore;

public partial class MainForm : Form
{
    private Control? _currentView;

    // ── DATA STRUCTURE: Stack<ViewName> ──────────────────────────────
    // LIFO — the last page visited is the first one Back returns to.
    // Using ViewName enum instead of strings: a typo is a compile error.
    private readonly Stack<ViewName> _navHistory = new();

    public MainForm()
    {
        InitializeComponent();
        ApplySession();
        ShowCatalog();
    }

    // ── Navigation ───────────────────────────────────────────────────
    private void ShowCatalog() => LoadView(new CatalogControl(), ViewName.Catalog);
    private void ShowCart()    => LoadView(new CartControl(),    ViewName.Cart);
    private void ShowOrders()  => LoadView(new OrdersControl(),  ViewName.Orders);
    private void ShowAdmin()   => LoadView(new AdminControl(),   ViewName.Admin);

    private void LoadView(Control view, ViewName name)
    {
        if (_currentView is not null)
        {
            if (_currentView.Tag is ViewName current)
                _navHistory.Push(current);           // Stack.Push — O(1)

            pnlContent.Controls.Remove(_currentView);
            _currentView.Dispose();
        }

        view.Dock = DockStyle.Fill;
        view.Tag  = name;
        pnlContent.Controls.Add(view);
        _currentView    = view;
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
        if (_navHistory.Count == 0) return;
        ViewName previous = _navHistory.Pop();           // Stack.Pop — O(1)

        if (_currentView is not null)
        {
            pnlContent.Controls.Remove(_currentView);
            _currentView.Dispose();
            _currentView = null;
        }

        Control view = previous switch
        {
            ViewName.Catalog => new CatalogControl(),
            ViewName.Cart    => new CartControl(),
            ViewName.Orders  => new OrdersControl(),
            ViewName.Admin   => new AdminControl(),
            _                => new CatalogControl(),
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
