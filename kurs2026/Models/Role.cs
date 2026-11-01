using System.Collections.Generic;

namespace AIS_Products_UI.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AccessRights { get; set; }
        public ICollection<User> Users { get; set; }
    }
}