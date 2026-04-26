// ══════════════════════════════════════════════════════════════════════
// ALGORITHM TECHNIQUE: REGEX — one of the ≥3 required techniques
// ══════════════════════════════════════════════════════════════════════
using ComputerStore.Data.Models;
using ComputerStore.Data.Models.Enums;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using ComputerStore.Infrastructure;
using static ComputerStore.Common.EntityConstants.User;

namespace ComputerStore;

public partial class RegisterForm : Form
{
    public RegisterForm() => InitializeComponent();

    private void BtnCreate_Click(object sender, EventArgs e)
    {
        lblError.Text = string.Empty;

        string username = txtUsername.Text.Trim();
        string email    = txtEmail.Text.Trim();
        string pass     = txtPass.Text;
        string confirm  = txtConfirm.Text;

        // Limits from EntityConstants — no hardcoded magic numbers
        if (username.Length < UsernameMinLength || username.Length > UsernameMaxLength)
        { ShowError($"Username must be {UsernameMinLength}–{UsernameMaxLength} characters."); return; }

        // ALGORITHM TECHNIQUE: REGEX
        // ^[a-zA-Z0-9_]+$ — letters, digits and underscores only
        if (!Regex.IsMatch(username, @"^[a-zA-Z0-9_]+$"))
        { ShowError("Username may only contain letters, digits and underscores."); return; }

        // REGEX: e-mail format — local@domain.tld
        if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        { ShowError("Please enter a valid e-mail address."); return; }

        if (pass.Length < 6)       { ShowError("Password must be at least 6 characters."); return; }
        if (pass != confirm)       { ShowError("Passwords do not match.");                 return; }

        try
        {
            if (ServiceLocator.Users.UsernameExists(username))
            { ShowError("That username is already taken."); return; }

            if (ServiceLocator.Users.EmailExists(email))
            { ShowError("That e-mail is already registered."); return; }

            ServiceLocator.Users.Register(new User
            {
                Username     = username,
                Email        = email,
                PasswordHash = HashPassword(pass),
                Role         = UserRole.Customer,
                CreatedAt    = DateTime.UtcNow,
            });

            MessageBox.Show("Account created! You can now log in.", "Success",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex) { ShowError($"Error: {ex.Message}"); }
    }

    private void ChkShow_CheckedChanged(object sender, EventArgs e)
    {
        char ch = chkShow.Checked ? '\0' : '●';
        txtPass.PasswordChar    = ch;
        txtConfirm.PasswordChar = ch;
    }

    private void ShowError(string msg) { lblError.Text = msg; lblError.ForeColor = AppColors.ErrorRed; }

    private static string HashPassword(string pw)
        => Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(pw)));
}
