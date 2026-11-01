using System;
using System.Collections.Generic;

namespace AIS_Products_UI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public DateTime? LastLoginAt { get; set; }

        public ICollection<Product> CreatedProducts { get; set; }
        public ICollection<ActionLog> ActionLogs { get; set; }
    }
}