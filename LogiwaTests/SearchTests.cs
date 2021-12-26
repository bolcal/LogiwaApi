using LogiwaApi.Data;
using LogiwaApi.Data.Entities;
using LogiwaApi.Services.Product.Query;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace LogiwaTests
{
    public class SearchTests
    {
        private readonly DbContextOptions<LgwDbContext> _dbContextOptions = new DbContextOptionsBuilder<LgwDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid()
            .ToString())
            .Options;
        public SearchTests()
        {
            var categories = new List<Category> {
                new Category { Id = 1, Name = "Headphones", MinStockToLive = 10 },
                new Category { Id = 2, Name = "Mouse", MinStockToLive = 50 },
                new Category { Id = 3, Name = "Keyboard", MinStockToLive = 10 },
                new Category { Id = 4, Name = "Monitor", MinStockToLive = 10 }
                };

            using var context = new LgwDbContext(_dbContextOptions);
            context.Categories.AddRange(categories);
            context.SaveChanges();
        }
        [Fact]
        public async Task GivenThereAreAvailableProductsInStock_WhenUserSearchForTheProduct_ThenFoundProductsShouldReturn()
        {
            using var context = new LgwDbContext(_dbContextOptions);
            context.Products.Add(new Product { Id = 1, Title = "Logitech M150", Description = "wired multimedia device", CategoryId = 3, StockQuantity = 60 });
            context.Products.Add(new Product { Id = 2, Title = "Fare", Description = "Test Description", CategoryId = 2, StockQuantity = 100 });
            context.Products.Add(new Product { Id = 3, Title = "LG Monitor", Description = "has mouse holder", CategoryId = 4, StockQuantity = 50 });
            await context.SaveChangesAsync();
            var sut = new SearchProduct.Handler(context);
            var searchRequest = new SearchProduct.Request()
            {
                SearchKey = "Mouse"
            };
            var response = await sut.Handle(searchRequest, CancellationToken.None);
            response.SearchResults.Count().ShouldBe(2);
            response.SearchResults[0].Title.ShouldBe("Fare");
            response.SearchResults[1].Title.ShouldBe("LG Monitor");
        }

        [Fact]
        public async Task GivenThereAreAvailableProductsInStock_WhenUserSearchForTheProduct_ThenFoundProductsHavingMinStockQuantityToBeLive()
        {
            using var context = new LgwDbContext(_dbContextOptions);
            context.Products.Add(new Product { Id = 1, Title = "Sony WF-1000", Description = "bluetooth device", CategoryId = 1, StockQuantity = 60 });
            context.Products.Add(new Product { Id = 2, Title = "Phillips T90", Description = "over ear headphone", CategoryId = 1, StockQuantity = 100 });
            context.Products.Add(new Product { Id = 3, Title = "Bose a19", Description = "high quality multimedia device", CategoryId = 1, StockQuantity = 9 });
            await context.SaveChangesAsync();
            var sut = new SearchProduct.Handler(context);
            var searchRequest = new SearchProduct.Request()
            {
                SearchKey = "headp"
            };
            var response = await sut.Handle(searchRequest, CancellationToken.None);
            response.SearchResults.Count().ShouldBe(2);
            response.SearchResults[0].Title.ShouldBe("Sony WF-1000");
            response.SearchResults[1].Title.ShouldBe("Phillips T90");
        }

        [Fact]
        public async Task GivenThereAreNotCategorizedProductsInStock_WhenUserSearchForTheProduct_ThenNotCategorizedProductsShouldnotReturn()
        {
            using var context = new LgwDbContext(_dbContextOptions);
            context.Products.Add(new Product { Id = 1, Title = "Mouse 1", Description = "bluetooth device", StockQuantity = 60 });
            context.Products.Add(new Product { Id = 2, Title = "Mouse 2", Description = "wired mouse device", StockQuantity = 100 });
            context.Products.Add(new Product { Id = 3, Title = "Mouse 3", CategoryId = 2, Description = "ergonomic mouse", StockQuantity = 50 });
            await context.SaveChangesAsync();
            var sut = new SearchProduct.Handler(context);
            var searchRequest = new SearchProduct.Request()
            {
                SearchKey = "mouse"
            };
            var response = await sut.Handle(searchRequest, CancellationToken.None);
            response.SearchResults.Count().ShouldBe(1);
            response.SearchResults[0].Title.ShouldBe("Mouse 3");
        }

        [Fact]
        public async Task GivenThereAreAvailableProductsInStock_WhenUserSearchForTheProductByMinMaxStockProps_ThenFoundProductsShouldBeValid()
        {
            using var context = new LgwDbContext(_dbContextOptions);
            context.Products.Add(new Product { Id = 1, Title = "Mouse 1", CategoryId = 2, StockQuantity = 60 });
            context.Products.Add(new Product { Id = 2, Title = "Mouse 2", CategoryId = 2, StockQuantity = 100 });
            context.Products.Add(new Product { Id = 3, Title = "Mouse 3", CategoryId = 2, StockQuantity = 120 });
            context.Products.Add(new Product { Id = 4, Title = "Mouse 4", CategoryId = 2, StockQuantity = 200 });
            context.Products.Add(new Product { Id = 5, Title = "Mouse 5", CategoryId = 2, StockQuantity = 500 });

            await context.SaveChangesAsync();
            var sut = new SearchProduct.Handler(context);
            var searchRequest = new SearchProduct.Request()
            {
                MinStock = 120,
                MaxStock = 450
            };
            var response = await sut.Handle(searchRequest, CancellationToken.None);
            response.SearchResults.Count().ShouldBe(2);
            response.SearchResults[0].Title.ShouldBe("Mouse 3");
            response.SearchResults[1].Title.ShouldBe("Mouse 4");
        }

    }
}