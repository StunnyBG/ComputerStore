using ComputerStore.Data.Models;

namespace ComputerStore.Services.Interfaces;

public interface IUserService
{
    /// <summary>Returns the user if credentials match, otherwise null.</summary>
    User? Authenticate(string username, string passwordHash);

    bool UsernameExists(string username);
    bool EmailExists(string email);
    void Register(User user);

    List<User> GetAll();
    void       ToggleRole(int userId);
}
