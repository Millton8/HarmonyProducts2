using HarmonyAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace HarmonyAPI.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Manufacturer> Manufacturers { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Stock> Stocks { get; set; } = null!;
        public DbSet<StockDetail> StocksDetail { get; set; } = null!;

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();


        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Первоначальная инициализация бд
            
            modelBuilder.Entity<Category>().HasData(
                    new Category {Id= Guid.NewGuid(), Name = "От насморка"},
                    new Category { Id = Guid.NewGuid(), Name = "От кашля"},
                    new Category { Id = Guid.NewGuid(), Name = "Для печени"}
            );

            modelBuilder.Entity<Manufacturer>().HasData(
                    new Manufacturer { Id = Guid.NewGuid(), Name = "Фавика" },
                    new Manufacturer {Id = Guid.NewGuid(), Name = "Гармоника" },
                    new Manufacturer { Id = Guid.NewGuid(), Name = "Фармацевт" }
            );
            modelBuilder.Entity<StockDetail>().HasData(
                    new StockDetail { Id = Guid.NewGuid(), Name = "Аптека 1",
                        City = "Красноярск", Adress = "Павлова 27" },
                    new StockDetail { Id = Guid.NewGuid(), Name = "Аптека 2",
                        City = "Красноярск", Adress = "Мира 1" },
                    new StockDetail { Id = Guid.NewGuid(), Name = "Склад",
                        City = "Красноярск", Adress = "Пушкина 8", IsMainStock = true }
            );
            //Устанавливаем уникальные атрибуты
            //На складе он составной чтобы нельзя было два раза добавить один и тот же продукт
            modelBuilder.Entity<Stock>().HasIndex(s => new { s.ProductId, s.DetailId })
                                        .IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.Login)
                                       .IsUnique();
            modelBuilder.Entity<Product>().HasIndex(u => u.Name)
                                       .IsUnique();




        }
    }
}
