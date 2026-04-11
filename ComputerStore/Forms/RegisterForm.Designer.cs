using ComputerStore.Infrastructure;

namespace ComputerStore
{
    partial class RegisterForm
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
            Text            = "Create Account";
            Size            = new Size(460, 590);
            StartPosition   = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox     = false;
            BackColor       = AppColors.Background;
            Font            = new Font("Segoe UI", 9.5f);

            // ── Header ───────────────────────────────────────────────
            pnlHeader = new Panel
            {
                Dock      = DockStyle.Top,
                Height    = 80,
                BackColor = AppColors.PrimaryBlue,
            };

            lblTitle = new Label
            {
                Text      = "Create a New Account",
                Font      = new Font("Segoe UI", 15, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize  = true,
                Location  = new Point(20, 22),
            };
            pnlHeader.Controls.Add(lblTitle);

            // ── Card ─────────────────────────────────────────────────
            pnlCard = new Panel
            {
                Size      = new Size(390, 450),
                Location  = new Point(32, 100),
                BackColor = AppColors.SurfaceWhite,
            };
            pnlCard.Paint += (s, e) => e.Graphics.DrawRectangle(
                new Pen(AppColors.BorderGray), 0, 0, pnlCard.Width - 1, pnlCard.Height - 1);

            // Username
            lblUsername          = UIFactory.FieldLabel("Username");
            lblUsername.Location = new Point(28, 24);
            txtUsername          = UIFactory.StyledTextBox(320);
            txtUsername.Location = new Point(28, 46);

            // Email
            lblEmail          = UIFactory.FieldLabel("E-mail");
            lblEmail.Location = new Point(28, 90);
            txtEmail          = UIFactory.StyledTextBox(320);
            txtEmail.Location = new Point(28, 112);

            // Password
            lblPass          = UIFactory.FieldLabel("Password (min 6 characters)");
            lblPass.Location = new Point(28, 156);
            txtPass          = UIFactory.StyledTextBox(320, password: true);
            txtPass.Location = new Point(28, 178);

            // Confirm
            lblConfirm          = UIFactory.FieldLabel("Confirm Password");
            lblConfirm.Location = new Point(28, 222);
            txtConfirm          = UIFactory.StyledTextBox(320, password: true);
            txtConfirm.Location = new Point(28, 244);

            // Show checkbox
            chkShow = new CheckBox
            {
                Text     = "Show passwords",
                AutoSize = true,
                Location = new Point(28, 286),
                Font     = new Font("Segoe UI", 9f),
            };
            chkShow.CheckedChanged += ChkShow_CheckedChanged;

            // Error label
            lblError = new Label
            {
                Text      = string.Empty,
                Location  = new Point(28, 318),
                Size      = new Size(330, 40),
                Font      = new Font("Segoe UI", 9f, FontStyle.Italic),
                ForeColor = AppColors.ErrorRed,
            };

            // Buttons
            btnCreate          = UIFactory.PrimaryButton("Create Account", 160, 40);
            btnCreate.Location = new Point(28, 368);
            btnCreate.Click   += BtnCreate_Click;

            btnCancel          = UIFactory.SecondaryButton("Cancel", 110, 40);
            btnCancel.Location = new Point(200, 368);
            btnCancel.Click   += (s, e) => Close();

            pnlCard.Controls.Add(lblUsername);
            pnlCard.Controls.Add(txtUsername);
            pnlCard.Controls.Add(lblEmail);
            pnlCard.Controls.Add(txtEmail);
            pnlCard.Controls.Add(lblPass);
            pnlCard.Controls.Add(txtPass);
            pnlCard.Controls.Add(lblConfirm);
            pnlCard.Controls.Add(txtConfirm);
            pnlCard.Controls.Add(chkShow);
            pnlCard.Controls.Add(lblError);
            pnlCard.Controls.Add(btnCreate);
            pnlCard.Controls.Add(btnCancel);

            Controls.Add(pnlHeader);
            Controls.Add(pnlCard);
        }

        #endregion

        // ── Control declarations ─────────────────────────────────────
        private Panel    pnlHeader  = null!;
        private Label    lblTitle   = null!;
        private Panel    pnlCard    = null!;
        private Label    lblUsername = null!;
        private TextBox  txtUsername = null!;
        private Label    lblEmail    = null!;
        private TextBox  txtEmail    = null!;
        private Label    lblPass     = null!;
        private TextBox  txtPass     = null!;
        private Label    lblConfirm  = null!;
        private TextBox  txtConfirm  = null!;
        private CheckBox chkShow     = null!;
        private Label    lblError    = null!;
        private Button   btnCreate   = null!;
        private Button   btnCancel   = null!;
    }
}
