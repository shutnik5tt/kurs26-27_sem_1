using System.Windows;

namespace AIS_Products_UI.Pages
{
    public partial class InputDialog : Window
    {
        public string Answer => txtAnswer.Text;

        public InputDialog(string message)
        {
            InitializeComponent();
            lblMessage.Text = message;
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}