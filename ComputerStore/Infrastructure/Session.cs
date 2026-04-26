using ComputerStore.Data.Models;
using ComputerStore.Data.Models.Enums;

namespace ComputerStore.Infrastructure;

/// <summary>
/// Holds the currently authenticated user for the lifetime of the process.
/// </summary>
public static class Session
{
    public static User? CurrentUser { get; private set; }
    public static bool  IsLoggedIn  => CurrentUser is not null;
    public static bool  IsAdmin     => CurrentUser?.Role == UserRole.Admin;

    public static void Login(User user) => CurrentUser = user;
    public static void Logout()         => CurrentUser = null;
}
