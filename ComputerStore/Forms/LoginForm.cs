using ComputerStore.Data.Models;
using System.Security.Cryptography;
using System.Text;
using ComputerStore.Infrastructure;
using static ComputerStore.Common.EntityConstants.User;

namespace ComputerStore;

public partial class LoginForm : Form
{
    public User? LoggedInUser { get; private set; }

    public LoginForm() => InitializeComponent();

    private void BtnLogin_Click(object sender, EventArgs e)
    {
        lblError.Text = string.Empty;

        string username = txtUsername.Text.Trim();
        string password = txtPassword.Text;

        if (string.IsNullOrWhiteSpace(username))
        { ShowError("Please enter your username."); txtUsername.Focus(); return; }

        if (string.IsNullOrWhiteSpace(password))
        { ShowError("Please enter your password."); txtPassword.Focus(); return; }

        // Validation limits pulled from EntityConstants — no magic numbers
        if (username.Length < UsernameMinLength || username.Length > UsernameMaxLength)
        { ShowError($"Username must be {UsernameMinLength}–{UsernameMaxLength} characters."); return; }

        try
        {
            var user = ServiceLocator.Users.Authenticate(username, HashPassword(password));
            if (user is null) { ShowError("Invalid username or password."); return; }

            LoggedInUser = user;
            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex) { ShowError($"Database error: {ex.Message}"); }
    }

    private void BtnRegister_Click(object sender, EventArgs e)
    {
        using var reg = new RegisterForm();
        reg.ShowDialog(this);
    }

    private void ChkShow_CheckedChanged(object sender, EventArgs e)
        => txtPassword.PasswordChar = chkShow.Checked ? '\0' : '●';

    private void TxtUsername_KeyDown(object sender, KeyEventArgs e)
    { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; txtPassword.Focus(); } }

    private void TxtPassword_KeyDown(object sender, KeyEventArgs e)
    { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; BtnLogin_Click(sender, e); } }

    private void ShowError(string msg) { lblError.Text = msg; lblError.ForeColor = AppColors.ErrorRed; }

    private static string HashPassword(string pw)
        => Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(pw)));
}
