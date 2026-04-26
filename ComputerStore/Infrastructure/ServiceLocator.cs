using ComputerStore.Services.Implementations;

namespace ComputerStore.Infrastructure;

/// <summary>
/// Holds one instance of every service used by the application.
/// A lightweight substitute for a DI container, appropriate for WinForms.
///
/// Usage anywhere in the project:
///   ServiceLocator.Parts.GetAll()
///   ServiceLocator.Cart.TryAdd(...)
/// </summary>
public static class ServiceLocator
{
    // Services from the ComputerStore.Services project
    public static IPartService  Parts  { get; } = new PartService();
    public static IOrderService Orders { get; } = new OrderService();
    public static IUserService  Users  { get; } = new UserService();

    // CartService lives in the WinForms project (Services/) because
    // it is pure in-memory UI state — it never touches the database.
    public static CartService Cart { get; } = new CartService();
}
