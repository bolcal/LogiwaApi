using LogiwaApi.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LogiwaApi.Data
{
    public class LgwDbContext :DbContext
    {
        public LgwDbContext(DbContextOptions<LgwDbContext> options) : base(options)
        {

        }

        public DbSet<Product> Products{ get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().Property(p => p.StockQuantity).HasDefaultValue(0);
            modelBuilder.Entity<Category>().Property(c=>c.MinStockToLive).HasDefaultValue(0);

            #region Seed
            modelBuilder.Entity<Category>().HasData(
               new Category { Id = 1, Name = "Headphones", MinStockToLive = 10 },
               new Category { Id = 2, Name = "Mouse", MinStockToLive = 50 },
               new Category { Id = 3, Name = "Keyboard", MinStockToLive = 10 },
               new Category { Id = 4, Name = "Monitor", MinStockToLive = 10 }
               );

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Title = "Sony WF-1000",Description="Bluetooth headphones with noise cancelling In-Ear", CategoryId = 1, StockQuantity = 20 },
                new Product { Id = 2, Title = "Phillips T90", Description ="Over-the-Ear Headphones", CategoryId = 1, StockQuantity = 40 },
                new Product { Id = 3, Title = "Logitech M150", Description ="Wired Mouse Optical Tracking", StockQuantity = 100 },
                new Product { Id = 4, Title = "Logitech Internet Pro", CategoryId = 3, StockQuantity = 4 },
                new Product { Id = 5, Title = "Asus Rog", CategoryId = 3, StockQuantity = 8 },
                new Product { Id = 6, Title = "LG 29WL500", CategoryId = 4, Description="Ultrawide monitor 75hz Full HD", StockQuantity = 50 },
                new Product { Id = 7, Title = "Logitech G502",Description="Wired Gaming Mouse Optical Tracking 8000 DPI ", CategoryId = 2, StockQuantity = 30 },
                new Product { Id = 8, Title = "Msi GG GM08",Description="Optical Wireless Gaming Mouse", CategoryId = 2, StockQuantity = 55 },
                new Product { Id = 9, Title = "HP X34", CategoryId = 4, Description = "IPS 165Hz HDR Gaming Monitor", StockQuantity = 25 }
                );
            #endregion
        }

    }
}
