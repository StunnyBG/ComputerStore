namespace ComputerStore.Infrastructure
{
    // ══════════════════════════════════════════════════════════════════════
    // OOP REQUIREMENT — ABSTRACTION & INHERITANCE
    // This abstract class defines the common contract for every view panel.
    // CatalogControl, CartControl, OrdersControl and AdminControl all
    // inherit from BaseControl, which itself extends UserControl.
    //
    // Principles demonstrated here:
    //   • Abstraction  — abstract LoadData() forces subclasses to implement
    //                    their own data loading without exposing HOW it works
    //   • Inheritance  — subclasses reuse ShowInfo / Confirm without
    //                    copy-pasting the MessageBox code
    //   • Polymorphism — RefreshView() is virtual; subclasses may override
    //                    it to add extra behaviour (e.g. rebuild dropdowns)
    //   • Encapsulation— shared helpers are protected so only the hierarchy
    //                    can call them; callers see only the public surface
    // ══════════════════════════════════════════════════════════════════════
    public abstract class BaseControl : UserControl   // INHERITANCE from UserControl
    {
        // ── ABSTRACTION: every view must know how to load its own data ──
        public abstract void LoadData();              // ABSTRACT METHOD — no body here

        // ── POLYMORPHISM: default refresh = reload; subclasses can extend ─
        public virtual void RefreshView() => LoadData();   // VIRTUAL — overridable

        // ── ENCAPSULATION: shared helpers behind protected access ────────
        protected static void ShowInfo(string msg) =>
            MessageBox.Show(msg, "Info",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

        protected static bool Confirm(string msg) =>
            MessageBox.Show(msg, "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;

        protected static void ShowError(string msg) =>
            MessageBox.Show(msg, "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
