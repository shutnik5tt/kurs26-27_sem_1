using System;
using System.Linq;
using System.Windows;
using AIS_Products_UI.Data;
using AIS_Products_UI.Models;

namespace AIS_Products_UI
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            using (var context = new AppDbContext())
            {
                // Создаем БД, если ее нет
                context.Database.EnsureCreated();

                // Если товаров нет, добавляем тестовые
                if (!context.Products.Any())
                {
                    var p1 = new Product
                    {
                        Name = "Насос промышленный",
                        Article = "PRD-001",
                        CategoryId = 1,
                        Characteristics = "Мощность: 5 кВт, Напряжение: 380В",
                        Price = 15000,
                        Description = "Промышленный насос для перекачки жидкостей",
                        Status = "В наличии",
                        CreatedBy = 1,
                        CreatedAt = DateTime.Now
                    };
                    var p2 = new Product
                    {
                        Name = "Клапан обратный",
                        Article = "PRD-002",
                        CategoryId = 2,
                        Characteristics = "Диаметр: 50 мм, Материал: сталь",
                        Price = 3500,
                        Description = "Обратный клапан для трубопровода",
                        Status = "В наличии",
                        CreatedBy = 1,
                        CreatedAt = DateTime.Now
                    };

                    context.Products.Add(p1);
                    context.Products.Add(p2);
                    context.SaveChanges();

                    // Добавляем остатки для этих товаров
                    context.StockHistories.Add(new StockHistory { ProductId = p1.Id, Quantity = 10, ChangeType = "Поступление", ChangedBy = 2, ChangedAt = DateTime.Now });
                    context.StockHistories.Add(new StockHistory { ProductId = p2.Id, Quantity = 5, ChangeType = "Поступление", ChangedBy = 2, ChangedAt = DateTime.Now });
                    context.SaveChanges();
                }
            }
        }
    }
}