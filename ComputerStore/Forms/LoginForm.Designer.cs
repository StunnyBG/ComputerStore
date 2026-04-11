using ComputerStore.Infrastructure;

namespace ComputerStore
{
    partial class LoginForm
    {
        private System.ComponentModel.IContainer components = null!;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            // ── Form ────────────────────────────────────────────────
            Text            = "ComputerStore – Login";
            Size            = new Size(460, 540);
            StartPosition   = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox     = false;
            BackColor       = AppColors.Background;
            Font            = new Font("Segoe UI", 9.5f);

            // ── Header panel ─────────────────────────────────────────
            pnlHeader = new Panel
            {
                Dock      = DockStyle.Top,
                Height    = 110,
                BackColor = AppColors.PrimaryBlue,
            };

            lblTitle = new Label
            {
                Text      = "💻 ComputerStore",
                Font      = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize  = true,
                Location  = new Point(20, 20),
            };

            lblSubtitle = new Label
            {
                Text      = "Sign in to your account",
                Font      = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(200, 220, 255),
                AutoSize  = true,
                Location  = new Point(22, 64),
            };

            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblSubtitle);

            // ── Card panel ───────────────────────────────────────────
            pnlCard = new Panel
            {
                Size      = new Size(380, 340),
                Location  = new Point(38, 130),
                BackColor = AppColors.SurfaceWhite,
            };
            pnlCard.Paint += (s, e) => e.Graphics.DrawRectangle(
                new Pen(AppColors.BorderGray), 0, 0, pnlCard.Width - 1, pnlCard.Height - 1);

            // Username
            lblUsername          = UIFactory.FieldLabel("Username");
            lblUsername.Location = new Point(30, 30);

            txtUsername              = UIFactory.StyledTextBox(300);
            txtUsername.Location     = new Point(30, 52);
            txtUsername.KeyDown     += TxtUsername_KeyDown;

            // Password
            lblPassword          = UIFactory.FieldLabel("Password");
            lblPassword.Location = new Point(30, 98);

            txtPassword              = UIFactory.StyledTextBox(300, password: true);
            txtPassword.Location     = new Point(30, 120);
            txtPassword.KeyDown     += TxtPassword_KeyDown;

            chkShow = new CheckBox
            {
                Text     = "Show password",
                Location = new Point(30, 158),
                AutoSize = true,
                Font     = new Font("Segoe UI", 9f),
            };
            chkShow.CheckedChanged += ChkShow_CheckedChanged;

            // Error label
            lblError = new Label
            {
                Text         = string.Empty,
                Font         = new Font("Segoe UI", 9f, FontStyle.Italic),
                ForeColor    = AppColors.ErrorRed,
                Location     = new Point(30, 188),
                Size         = new Size(320, 36),
                AutoEllipsis = true,
            };

            // Buttons
            btnLogin          = UIFactory.PrimaryButton("Login", 140, 40);
            btnLogin.Location = new Point(30, 234);
            btnLogin.Click   += BtnLogin_Click;

            btnRegister          = UIFactory.SecondaryButton("Register", 140, 40);
            btnRegister.Location = new Point(184, 234);
            btnRegister.Click   += BtnRegister_Click;

            pnlCard.Controls.Add(lblUsername);
            pnlCard.Controls.Add(txtUsername);
            pnlCard.Controls.Add(lblPassword);
            pnlCard.Controls.Add(txtPassword);
            pnlCard.Controls.Add(chkShow);
            pnlCard.Controls.Add(lblError);
            pnlCard.Controls.Add(btnLogin);
            pnlCard.Controls.Add(btnRegister);

            // Footer
            lblFooter = new Label
            {
                Text      = "© 2025 ComputerStore",
                Font      = new Font("Segoe UI", 8f),
                ForeColor = Color.Gray,
                AutoSize  = true,
                Location  = new Point(160, 495),
            };

            Controls.Add(pnlHeader);
            Controls.Add(pnlCard);
            Controls.Add(lblFooter);
        }

        #endregion

        // ── Control declarations ─────────────────────────────────────
        private Panel    pnlHeader   = null!;
        private Label    lblTitle    = null!;
        private Label    lblSubtitle = null!;
        private Panel    pnlCard     = null!;
        private Label    lblUsername = null!;
        private TextBox  txtUsername = null!;
        private Label    lblPassword = null!;
        private TextBox  txtPassword = null!;
        private CheckBox chkShow     = null!;
        private Button   btnLogin    = null!;
        private Button   btnRegister = null!;
        private Label    lblError    = null!;
        private Label    lblFooter   = null!;
    }
}
