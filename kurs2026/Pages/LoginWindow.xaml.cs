using System.Linq;
using System.Windows;
using AIS_Products_UI.Data;

namespace AIS_Products_UI.Pages
{
    public partial class LoginWindow : Window
    {
        public LoginWindow() { InitializeComponent(); }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string login = txtLogin.Text.Trim();
            string password = txtPassword.Password.Trim();

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                lblError.Text = "Заполните все поля!";
                return;
            }

            using (var context = new AppDbContext())
            {
                var user = context.Users.FirstOrDefault(u => u.Login == login && u.PasswordHash == password);
                if (user != null)
                {
                    user.LastLoginAt = System.DateTime.Now;
                    context.SaveChanges();

                    var catalog = new CatalogWindow(user.Id, user.RoleId);
                    catalog.Show();
                    this.Close();
                }
                else
                {
                    lblError.Text = "Неверный логин или пароль!";
                }
            }
        }
    }
}