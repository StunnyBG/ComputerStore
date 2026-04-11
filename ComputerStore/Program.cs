using System.Globalization;

namespace ComputerStore
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // Force en-US culture for consistent decimal/date formatting
            Thread.CurrentThread.CurrentCulture   = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

            ApplicationConfiguration.Initialize();

            // Show login; only open the main shell on success
            using var login = new LoginForm();
            if (login.ShowDialog() == DialogResult.OK && login.LoggedInUser is not null)
            {
                Session.Login(login.LoggedInUser);
                Application.Run(new MainForm());
            }
        }
    }
}
