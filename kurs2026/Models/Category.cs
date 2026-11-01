using System.Collections.Generic;

namespace AIS_Products_UI.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public Category Parent { get; set; }
        public ICollection<Category> Children { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}