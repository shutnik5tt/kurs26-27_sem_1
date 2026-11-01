using System;
using System.Collections.Generic;

namespace AIS_Products_UI.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Article { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public string Characteristics { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }

        public int CreatedBy { get; set; }
        public User Creator { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<StockHistory> StockHistories { get; set; }
    }
}