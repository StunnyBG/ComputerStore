using ComputerStore.Data;
using ComputerStore.Data.Models;
using ComputerStore.Data.Models.Enums;
using ComputerStore.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ComputerStore.Services.Implementations;

public class UserService : IUserService
{
    public User? Authenticate(string username, string passwordHash)
    {
        using var ctx = DbContextFactory.Create();
        return ctx.Users.FirstOrDefault(
            u => u.Username == username && u.PasswordHash == passwordHash);
    }

    public bool UsernameExists(string username)
    {
        using var ctx = DbContextFactory.Create();
        return ctx.Users.Any(u => u.Username == username);
    }

    public bool EmailExists(string email)
    {
        using var ctx = DbContextFactory.Create();
        return ctx.Users.Any(u => u.Email == email);
    }

    public void Register(User user)
    {
        using var ctx = DbContextFactory.Create();
        ctx.Users.Add(user);
        ctx.SaveChanges();
    }

    public List<User> GetAll()
    {
        using var ctx = DbContextFactory.Create();
        return ctx.Users.AsNoTracking().OrderBy(u => u.Username).ToList();
    }

    public void ToggleRole(int userId)
    {
        using var ctx = DbContextFactory.Create();
        var user = ctx.Users.Find(userId)
                   ?? throw new InvalidOperationException("User not found.");
        user.Role = user.Role == UserRole.Admin ? UserRole.Customer : UserRole.Admin;
        ctx.SaveChanges();
    }
}
