using System;

namespace AIS_Products_UI.Models
{
    public class StockHistory
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public string ChangeType { get; set; }
        public DateTime ChangedAt { get; set; }
        public int ChangedBy { get; set; }
        public User Changer { get; set; }
    }
}