using System;
using System.IO;
using System.Linq;
using System.Windows;
using AIS_Products_UI.Data;
using AIS_Products_UI.Models;

namespace AIS_Products_UI.Pages
{
    public partial class ImportWindow : Window
    {
        private string _filePath = "";

        public ImportWindow() { InitializeComponent(); }

        private void BtnSelectFile_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "CSV files (*.csv)|*.csv";
            if (dialog.ShowDialog() == true)
            {
                _filePath = dialog.FileName;
                lblFileName.Text = Path.GetFileName(_filePath);
                lblFileName.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        private void BtnImport_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_filePath))
            {
                MessageBox.Show("Выберите файл!");
                return;
            }

            try
            {
                string[] lines = File.ReadAllLines(_filePath);
                int importedCount = 0;
                string errors = "";

                using (var context = new AppDbContext())
                {
                    for (int i = 1; i < lines.Length; i++)
                    {
                        string[] parts = lines[i].Split(';');
                        if (parts.Length < 4) { errors += $"Строка {i + 1}: неверный формат\n"; continue; }

                        string article = parts[0].Trim();
                        string name = parts[1].Trim();
                        int catId = int.Parse(parts[2].Trim());
                        decimal price = decimal.Parse(parts[3].Trim());

                        if (context.Products.Any(p => p.Article == article))
                        {
                            errors += $"Строка {i + 1}: артикул {article} уже есть\n";
                            continue;
                        }

                        var product = new Product
                        {
                            Name = name,
                            Article = article,
                            CategoryId = catId,
                            Price = price,
                            Status = "В наличии",
                            CreatedBy = 1,
                            CreatedAt = DateTime.Now
                        };
                        context.Products.Add(product);
                        context.SaveChanges();
                        importedCount++;
                    }
                }

                lblStatus.Text = $"Импортировано: {importedCount}\n{errors}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e) => this.Close();
    }
}