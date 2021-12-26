using LogiwaApi.Data;
using LogiwaApi.Data.Entities;
using LogiwaApi.Exceptions;
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
    public class ProductUpdateTests
    {
        private readonly DbContextOptions<LgwDbContext> _dbContextOptions = new DbContextOptionsBuilder<LgwDbContext>()
           .UseInMemoryDatabase(databaseName: Guid.NewGuid()
           .ToString())
           .Options;

        public ProductUpdateTests()
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
        public async Task GivenThereIsSingleProductInDatabase_WhenNewProductTriedToUpdateByEmptyTitle_ThenInvalidProductTitleExceptionThrow()
        {
            using var context = new LgwDbContext(_dbContextOptions);
            context.Products.Add(new Product { Title = "LG Monitor", Description = "has mouse holder", CategoryId = 4, StockQuantity = 50 });
            await context.SaveChangesAsync();
            var productRequest = new UpdateProduct.Request
            {
                Title = String.Empty,
                CategoryId = 4,
                Description = "test desc",
                StockQuantity = 1,
                Id = 1,
            };
            var sut = new UpdateProduct.Handler(context);
            await Should.ThrowAsync<InvalidProductTitleException>(() => sut.Handle(productRequest, CancellationToken.None));
        }

        [Fact]
        public async Task WhenThereIsSingleProductInDatabase_WhenProductUpdate_ThenProductFieldsShouldBeUpdated()
        {
            using var context = new LgwDbContext(_dbContextOptions);
            context.Products.Add(new Product { Title = "LG Monitor", Description = "has mouse holder", CategoryId = 1, StockQuantity = 50 });
            await context.SaveChangesAsync();
            var productRequest = new UpdateProduct.Request
            {
                Title = "Title Updated",
                CategoryId = 4,
                Description = "Description Updated",
                StockQuantity = 100,
                Id = 1,
            };
            var sut = new UpdateProduct.Handler(context);
            await sut.Handle(productRequest, CancellationToken.None);
            var productUpdated = await context.Products.FindAsync(1);
            productUpdated.Title.ShouldBe("Title Updated");
            productUpdated.CategoryId.ShouldBe(4);
            productUpdated.Description.ShouldBe("Description Updated");
            productUpdated.StockQuantity.ShouldBe(100);
        }
    }
}
