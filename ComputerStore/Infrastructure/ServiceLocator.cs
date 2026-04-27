using ComputerStore.Services;
using ComputerStore.Services.Implementations;
using ComputerStore.Services.Interfaces;

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
    public static ICartService Cart { get; } = new CartService();
}
