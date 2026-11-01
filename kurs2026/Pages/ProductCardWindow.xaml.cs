using System.Linq;
using System.Windows;
using AIS_Products_UI.Data;

namespace AIS_Products_UI.Pages
{
    public partial class ProductCardWindow : Window
    {
        private int _productId;
        private int _userId;
        private int _roleId;

        public ProductCardWindow(int productId, int userId, int roleId)
        {
            InitializeComponent();
            _productId = productId;
            _userId = userId;
            _roleId = roleId;

            if (_roleId == 3)
            {
                btnUpdateStock.Visibility = Visibility.Collapsed;
            }

            LoadProductData();
        }

        private void LoadProductData()
        {
            using (var context = new AppDbContext())
            {
                var p = context.Products
                    .Where(x => x.Id == _productId)
                    .Select(x => new
                    {
                        x.Name,
                        x.Article,
                        CategoryName = x.Category.Name,
                        x.Price,
                        x.Description,
                        x.Characteristics,
                        Quantity = x.StockHistories.Where(s => s.ChangeType == "Поступление").Sum(s => s.Quantity) -
                                   x.StockHistories.Where(s => s.ChangeType == "Списание").Sum(s => s.Quantity)
                    })
                    .FirstOrDefault();

                if (p != null)
                {
                    lblTitle.Text = p.Name;
                    lblArticle.Text = "Артикул: " + p.Article;
                    lblCategory.Text = "Категория: " + p.CategoryName;
                    lblPrice.Text = "Цена: " + p.Price.ToString("N2") + " руб.";
                    lblDescription.Text = p.Description;
                    lblCharacteristics.Text = p.Characteristics;
                    lblQuantity.Text = p.Quantity + " шт.";
                }
            }
        }

        private void BtnUpdateStock_Click(object sender, RoutedEventArgs e)
        {
            var inputDialog = new InputDialog("Введите количество для поступления:");
            if (inputDialog.ShowDialog() == true)
            {
                if (int.TryParse(inputDialog.Answer, out int amount))
                {
                    using (var context = new AppDbContext())
                    {
                        var history = new Models.StockHistory
                        {
                            ProductId = _productId,
                            Quantity = amount,
                            ChangeType = "Поступление",
                            ChangedBy = _userId,
                            ChangedAt = System.DateTime.Now
                        };
                        context.StockHistories.Add(history);
                        context.SaveChanges();
                    }
                    LoadProductData();
                }
                else
                {
                    MessageBox.Show("Введите число!");
                }
            }
        }
    }
}