using System.Linq;
using System.Windows;
using AIS_Products_UI.Data;
using AIS_Products_UI.Models;

namespace AIS_Products_UI.Pages
{
    public partial class AddProductWindow : Window
    {
        private int _userId;

        public AddProductWindow(int userId)
        {
            InitializeComponent();
            _userId = userId;
            LoadCategories();
        }

        private void LoadCategories()
        {
            using (var context = new AppDbContext())
            {
                cmbCategory.ItemsSource = context.Categories.ToList();
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtArticle.Text) ||
                cmbCategory.SelectedValue == null || string.IsNullOrWhiteSpace(txtPrice.Text))
            {
                MessageBox.Show("Заполните обязательные поля!");
                return;
            }

            if (!decimal.TryParse(txtPrice.Text, out decimal price))
            {
                MessageBox.Show("Цена должна быть числом!");
                return;
            }

            using (var context = new AppDbContext())
            {
                if (context.Products.Any(p => p.Article == txtArticle.Text.Trim()))
                {
                    MessageBox.Show("Товар с таким артикулом уже существует!");
                    return;
                }

                var product = new Product
                {
                    Name = txtName.Text.Trim(),
                    Article = txtArticle.Text.Trim(),
                    CategoryId = (int)cmbCategory.SelectedValue,
                    Price = price,
                    Characteristics = txtCharacteristics.Text,
                    Description = txtDescription.Text,
                    Status = "В наличии",
                    CreatedBy = _userId,
                    CreatedAt = System.DateTime.Now
                };

                context.Products.Add(product);
                context.SaveChanges();

                var stock = new StockHistory
                {
                    ProductId = product.Id,
                    Quantity = 0,
                    ChangeType = "Поступление",
                    ChangedBy = _userId,
                    ChangedAt = System.DateTime.Now
                };
                context.StockHistories.Add(stock);
                context.SaveChanges();

                MessageBox.Show("Товар добавлен!");
                this.Close();
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e) => this.Close();
    }
}