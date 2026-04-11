using ComputerStore.Data.Models;
using ComputerStore.Data.Models.Enums;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace ComputerStore
{
    public partial class RegisterForm : Form
    {
        public RegisterForm()
        {
            InitializeComponent();
        }

        // ── Event handlers ────────────────────────────────────────────
        private void BtnCreate_Click(object sender, EventArgs e)
        {
            lblError.Text = string.Empty;

            string username = txtUsername.Text.Trim();
            string email    = txtEmail.Text.Trim();
            string pass     = txtPass.Text;
            string confirm  = txtConfirm.Text;

            if (username.Length < 3 || username.Length > 50)
            { ShowError("Username must be 3–50 characters."); return; }

            if (!Regex.IsMatch(username, @"^[a-zA-Z0-9_]+$"))
            { ShowError("Username may only contain letters, digits and underscores."); return; }

            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            { ShowError("Please enter a valid e-mail address."); return; }

            if (pass.Length < 6)
            { ShowError("Password must be at least 6 characters."); return; }

            if (pass != confirm)
            { ShowError("Passwords do not match."); return; }

            try
            {
                using var ctx = DbContextFactory.Create();

                if (ctx.Users.Any(u => u.Username == username))
                { ShowError("That username is already taken."); return; }

                if (ctx.Users.Any(u => u.Email == email))
                { ShowError("That e-mail is already registered."); return; }

                ctx.Users.Add(new User
                {
                    Username     = username,
                    Email        = email,
                    PasswordHash = HashPassword(pass),
                    Role         = UserRole.Customer,
                    CreatedAt    = DateTime.UtcNow,
                });
                ctx.SaveChanges();

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

        // ── Helpers ──────────────────────────────────────────────────
        private void ShowError(string msg)
        {
            lblError.Text      = msg;
            lblError.ForeColor = AppColors.ErrorRed;
        }

        private static string HashPassword(string pw)
            => Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(pw)));
    }
}
