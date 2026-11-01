using Microsoft.EntityFrameworkCore;
using AIS_Products_UI.Models;

namespace AIS_Products_UI.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<StockHistory> StockHistories { get; set; }
        public DbSet<ActionLog> ActionLogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=localhost;Initial Catalog=ProductsDB_EF;Integrated Security=True;TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(u => u.Login).IsUnique();
            modelBuilder.Entity<Product>().HasIndex(p => p.Article).IsUnique();

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Creator)
                .WithMany(u => u.CreatedProducts)
                .HasForeignKey(p => p.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StockHistory>()
                .HasOne(s => s.Changer)
                .WithMany()
                .HasForeignKey(s => s.ChangedBy)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ActionLog>()
                .HasOne(a => a.User)
                .WithMany(u => u.ActionLogs)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Администратор", AccessRights = "Все права" },
                new Role { Id = 2, Name = "Сотрудник отдела продаж", AccessRights = "Просмотр, добавление, редактирование" },
                new Role { Id = 3, Name = "Клиент", AccessRights = "Просмотр" }
            );

            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Login = "admin", PasswordHash = "12345", RoleId = 1 },
                new User { Id = 2, Login = "manager", PasswordHash = "12345", RoleId = 2 },
                new User { Id = 3, Login = "client", PasswordHash = "12345", RoleId = 3 }
            );

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Насосы" },
                new Category { Id = 2, Name = "Клапаны" },
                new Category { Id = 3, Name = "Электрика" }
            );
        }
    }
}