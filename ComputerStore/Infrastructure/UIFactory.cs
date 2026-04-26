namespace ComputerStore.Infrastructure;

/// <summary>
/// Produces consistently styled WinForms controls.
/// All visual defaults live here so individual forms stay lean.
/// </summary>
public static class UIFactory
{
    public static Button PrimaryButton(string text, int width = 140, int height = 38)
    {
        var btn = new Button
        {
            Text      = text, Width = width, Height = height,
            BackColor = AppColors.AccentYellow,
            ForeColor = AppColors.DarkBlue,
            FlatStyle = FlatStyle.Flat,
            Font      = new Font("Segoe UI", 9.5f, FontStyle.Bold),
            Cursor    = Cursors.Hand,
        };
        btn.FlatAppearance.BorderColor = AppColors.PrimaryBlue;
        btn.FlatAppearance.BorderSize  = 1;
        return btn;
    }

    public static Button SecondaryButton(string text, int width = 140, int height = 38)
    {
        var btn = new Button
        {
            Text      = text, Width = width, Height = height,
            BackColor = AppColors.SurfaceWhite,
            ForeColor = AppColors.PrimaryBlue,
            FlatStyle = FlatStyle.Flat,
            Font      = new Font("Segoe UI", 9.5f),
            Cursor    = Cursors.Hand,
        };
        btn.FlatAppearance.BorderColor = AppColors.PrimaryBlue;
        btn.FlatAppearance.BorderSize  = 1;
        return btn;
    }

    public static Label HeaderLabel(string text, int fontSize = 14) =>
        new Label
        {
            Text      = text,
            Font      = new Font("Segoe UI", fontSize, FontStyle.Bold),
            ForeColor = AppColors.PrimaryBlue,
            AutoSize  = true,
        };

    public static TextBox StyledTextBox(int width = 260, bool password = false)
    {
        var tb = new TextBox
        {
            Width       = width, Height = 28,
            Font        = new Font("Segoe UI", 10f),
            BorderStyle = BorderStyle.FixedSingle,
            BackColor   = AppColors.SurfaceWhite,
        };
        if (password) tb.PasswordChar = '●';
        return tb;
    }

    public static Label FieldLabel(string text) =>
        new Label
        {
            Text      = text,
            Font      = new Font("Segoe UI", 9.5f),
            ForeColor = AppColors.TextDark,
            AutoSize  = true,
        };

    public static DataGridView StyledGrid()
    {
        var grid = new DataGridView
        {
            Dock                  = DockStyle.Fill,
            AutoSizeColumnsMode   = DataGridViewAutoSizeColumnsMode.Fill,
            AllowUserToAddRows    = false,
            AllowUserToDeleteRows = false,
            ReadOnly              = true,
            SelectionMode         = DataGridViewSelectionMode.FullRowSelect,
            BackgroundColor       = AppColors.Background,
            BorderStyle           = BorderStyle.None,
            CellBorderStyle       = DataGridViewCellBorderStyle.SingleHorizontal,
            ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None,
            RowHeadersVisible     = false,
            Font                  = new Font("Segoe UI", 9.5f),
        };
        grid.ColumnHeadersDefaultCellStyle.BackColor = AppColors.PrimaryBlue;
        grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        grid.ColumnHeadersDefaultCellStyle.Font      = new Font("Segoe UI", 9.5f, FontStyle.Bold);
        grid.ColumnHeadersHeight = 36;
        grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        grid.DefaultCellStyle.SelectionBackColor = AppColors.LightYellow;
        grid.DefaultCellStyle.SelectionForeColor = AppColors.DarkBlue;
        grid.DefaultCellStyle.BackColor          = AppColors.SurfaceWhite;
        grid.DefaultCellStyle.Padding            = new Padding(4, 2, 4, 2);
        grid.RowTemplate.Height = 30;
        grid.AlternatingRowsDefaultCellStyle.BackColor = AppColors.Background;
        return grid;
    }
}
