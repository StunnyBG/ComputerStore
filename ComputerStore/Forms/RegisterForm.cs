// ══════════════════════════════════════════════════════════════════════
// VALIDATION & REGEX REQUIREMENT
// This form validates user input with both length checks and regular
// expressions.  Regex is one of the ≥3 required algorithm techniques.
// ══════════════════════════════════════════════════════════════════════
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

            // ── INPUT VALIDATION — length checks ─────────────────────
            // REQUIREMENT: verifikatsiya na vkhodnite danni (input verification)
            if (username.Length < 3 || username.Length > 50)
            { ShowError("Username must be 3–50 characters."); return; }

            // ── ALGORITHM TECHNIQUE: REGEX (Regular Expression) ──────
            // REQUIREMENT: regex е посочен като един от ≥3 алгоритъма/техники
            //
            // Pattern ^[a-zA-Z0-9_]+$ means:
            //   ^        — start of string
            //   [a-zA-Z0-9_]  — any letter, digit, or underscore
            //   +        — one or more occurrences
            //   $        — end of string
            // This rejects usernames with spaces, hyphens, symbols, etc.
            if (!Regex.IsMatch(username, @"^[a-zA-Z0-9_]+$"))
            { ShowError("Username may only contain letters, digits and underscores."); return; }

            // REGEX: e-mail validation
            // Pattern ^[^@\s]+@[^@\s]+\.[^@\s]+$ checks for:
            //   ^[^@\s]+   — one or more chars that are NOT @ or whitespace
            //   @          — the @ symbol
            //   [^@\s]+    — domain name part
            //   \.         — literal dot
            //   [^@\s]+$   — TLD (at least one char)
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

        // ENCAPSULATION: password hashing is hidden from the rest of the form
        private static string HashPassword(string pw)
            => Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(pw)));
    }
}
