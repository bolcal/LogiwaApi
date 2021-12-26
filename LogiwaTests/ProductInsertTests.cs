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
    public class ProductInsertTests
    {
        private readonly DbContextOptions<LgwDbContext> _dbContextOptions = new DbContextOptionsBuilder<LgwDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid()
            .ToString())
            .Options;

        public ProductInsertTests()
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
        public async Task GivenThereIsNoProductInTheDatabase_WhenNewProductAddedWithTitleExceedingMaxLength_ThenInvalidProductTitleExceptionShouldThrow()
        {
            using var context = new LgwDbContext(_dbContextOptions);
            var productRequest = new InsertProduct.Request
            {
                Title = "This title length is more than 200 characters, " +
                "bla bla bla bla bla bla bla blabla bla bla blabla bla bla blabla bla bla blabla bla bla" +
                " bla bla bla bla bla bla bla bla bla bla bla bla blabla bla bla bla",
                CategoryId = 1,
                Description = "test desc",
                StockQuantity = 1
            };
            var sut = new InsertProduct.Handler(context);
            await Should.ThrowAsync<InvalidProductTitleException>(() => sut.Handle(productRequest, CancellationToken.None));
        }

        [Fact]
        public async Task GivenThereIsNoProductInDatabase_WhenNewProductAdded_ThenNewProductShouldExists()
        {
            using var context = new LgwDbContext(_dbContextOptions);
            var productRequest = new InsertProduct.Request
            {
                Title = "TestTitle",
                CategoryId = 1,
                Description = "test desc",
                StockQuantity = 1
            };
            var sut = new InsertProduct.Handler(context);
            await sut.Handle(productRequest, CancellationToken.None);
            var productInserted = await context.Products.FindAsync(1);
            productInserted.ShouldNotBeNull();
            productInserted.Title.ShouldBe("TestTitle");
        }

        [Fact]
        public async Task GivenThereIsNoProductInDatabase_WhenNewProductAddedWithInvalidCategoryId_ThenInvalidCategoryExceptionShouldThrow()
        {
            using var context = new LgwDbContext(_dbContextOptions);
            var productRequest = new InsertProduct.Request
            {
                Title = "TestTitle",
                CategoryId = 10,
                Description = "test desc",
                StockQuantity = 1
            };
            var sut = new InsertProduct.Handler(context);
            await Should.ThrowAsync<InvalidCategoryException>(() => sut.Handle(productRequest, CancellationToken.None));
        }
    }
}
