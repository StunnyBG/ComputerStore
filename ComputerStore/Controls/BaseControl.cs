namespace ComputerStore;

// ══════════════════════════════════════════════════════════════════════
// OOP: ABSTRACTION & INHERITANCE
// Folder: Controls/ — BaseControl belongs here because it IS a control.
// Infrastructure/ is for plumbing (colors, session, service locator),
// not for UI base classes.
//
//   • Abstraction  — abstract LoadData() forces each view to implement
//                    its own data loading without exposing how it works
//   • Inheritance  — subclasses get ShowInfo / Confirm for free
//   • Polymorphism — RefreshView() is virtual; subclasses can extend it
//   • Encapsulation— helpers are protected, invisible to outside callers
// ══════════════════════════════════════════════════════════════════════
public abstract class BaseControl : UserControl
{
    public abstract void LoadData();
    public virtual  void RefreshView() => LoadData();

    protected static void ShowInfo(string msg) =>
        MessageBox.Show(msg, "Info",    MessageBoxButtons.OK,    MessageBoxIcon.Information);

    protected static bool Confirm(string msg) =>
        MessageBox.Show(msg, "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            == DialogResult.Yes;

    protected static void ShowError(string msg) =>
        MessageBox.Show(msg, "Error",   MessageBoxButtons.OK,    MessageBoxIcon.Error);
}
