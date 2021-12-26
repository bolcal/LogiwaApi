using LogiwaApi.Data.Entities;
using System.Data.Entity;
using static LogiwaApi.Services.Product.Query.SearchProduct;

namespace LogiwaApi.Infrastructure
{
    public static class MappingExtensions
    {
        public static async Task<IEnumerable<ProductSearchResult>> ToProductSearchResults(this IQueryable<Product> products)
        {
            return await products.Select(p => p.ToProductSearchResult()).ToListAsync();
        }
        public static ProductSearchResult ToProductSearchResult(this Product product)
        {
            return new ProductSearchResult
            {
                Id = product.Id,
                Category = product.Category !=null ? product.Category.Name : String.Empty,
                Description = product.Description ?? String.Empty,
                Stock =product.StockQuantity,
                Title = product.Title
            };
        }
    }
}
