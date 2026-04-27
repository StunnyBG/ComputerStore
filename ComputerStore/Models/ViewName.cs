namespace ComputerStore.Models;

/// <summary>
/// Identifies each top-level view for the navigation Stack.
/// Using an enum instead of raw strings means a typo is a compile error,
/// not a silent wrong-page navigation.
/// </summary>
public enum ViewName
{
    Catalog,
    Cart,
    Orders,
    Admin,
}
