using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace ComputerStore
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

            using (var ctx = DbContextFactory.Create())
                ctx.Database.Migrate();

            ApplicationConfiguration.Initialize();

            using var login = new LoginForm();
            if (login.ShowDialog() == DialogResult.OK && login.LoggedInUser is not null)
            {
                Session.Login(login.LoggedInUser);
                Application.Run(new MainForm());
            }
        }
    }
}