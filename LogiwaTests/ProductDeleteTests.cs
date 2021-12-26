using LogiwaApi.Data;
using LogiwaApi.Data.Entities;
using LogiwaApi.Services.Product.Command;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace LogiwaTests
{
    public class ProductDeleteTests
    {
        private readonly DbContextOptions<LgwDbContext> _dbContextOptions = new DbContextOptionsBuilder<LgwDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid()
            .ToString())
            .Options;
        public ProductDeleteTests()
        {
            using var context = new LgwDbContext(_dbContextOptions);
            context.Categories.Add(new Category { Id = 1, Name = "Headphones", MinStockToLive = 10 });
            context.SaveChanges();
            var products = new List<Product> {
                new Product { Id = 1,Title = "Product 1",CategoryId=1,Description="desc",StockQuantity=10},
                new Product { Id = 2,Title = "Product 2",CategoryId=1,Description="desc",StockQuantity=10},
                new Product { Id = 3,Title = "Product 3",CategoryId=1,Description="desc",StockQuantity=10}
                };
            context.Products.AddRange(products);
            context.SaveChanges();
        }

        [Fact]
        public async Task GivenThereIsAvailableProductsInDatabase_WhenDeleteProduct_ThenProductShouldBeRemoved()
        {
            using var context = new LgwDbContext(_dbContextOptions);
            var productRequest = new DeleteProduct.Request { Id = 1 };
            var sut = new DeleteProduct.Handler(context);
            await sut.Handle(productRequest, CancellationToken.None);
            var product = await context.Products.FindAsync(1);
            product.ShouldBeNull();
        }
    }
}
