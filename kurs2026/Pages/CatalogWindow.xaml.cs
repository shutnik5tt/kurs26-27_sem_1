using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using AIS_Products_UI.Data;

namespace AIS_Products_UI.Pages
{
    public partial class CatalogWindow : Window
    {
        private int _currentUserId;
        private int _currentUserRoleId;

        public CatalogWindow(int userId, int roleId)
        {
            InitializeComponent();
            _currentUserId = userId;
            _currentUserRoleId = roleId;

            if (_currentUserRoleId == 3)
                btnAddProduct.Visibility = Visibility.Collapsed;

            LoadProducts();
        }

        private void LoadProducts(string searchTerm = "")
        {
            using (var context = new AppDbContext())
            {
                var products = context.Products
                    .Where(p => p.Name.Contains(searchTerm) || p.Article.Contains(searchTerm))
                    .Select(p => new ProductView
                    {
                        Id = p.Id,
                        Article = p.Article,
                        Name = p.Name,
                        CategoryName = p.Category.Name,
                        Price = p.Price,
                        Status = p.Status,
                        Quantity = p.StockHistories.Where(s => s.ChangeType == "Поступление").Sum(s => s.Quantity) -
                                   p.StockHistories.Where(s => s.ChangeType == "Списание").Sum(s => s.Quantity)
                    })
                    .ToList();

                dgProducts.ItemsSource = products;
            }
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadProducts(txtSearch.Text);
        }

        private void BtnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddProductWindow(_currentUserId);
            addWindow.ShowDialog();
            LoadProducts();
        }

        private void BtnImport_Click(object sender, RoutedEventArgs e)
        {
            var importWindow = new ImportWindow();
            importWindow.ShowDialog();
            LoadProducts();
        }

        private void DgProducts_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (dgProducts.SelectedItem is ProductView selectedProduct)
            {
                var cardWindow = new ProductCardWindow(selectedProduct.Id, _currentUserId, _currentUserRoleId);
                cardWindow.ShowDialog();
                LoadProducts();
            }
        }
    }

    public class ProductView
    {
        public int Id { get; set; }
        public string Article { get; set; }
        public string Name { get; set; }
        public string CategoryName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
    }
}