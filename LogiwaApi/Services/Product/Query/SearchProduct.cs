using LogiwaApi.Data;
using LogiwaApi.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LogiwaApi.Services.Product.Query
{
    public static class SearchProduct
    {
        public class Request : IRequest<Response>
        {
            public string? SearchKey { get; set; }
            public int PageNum { get; set; } = 0;
            public int PageItemCount { get; set; } = 10;
            public int? MinStock { get; set; }
            public int? MaxStock { get; set; }
        }
        public class Response
        {
            public List<ProductSearchResult> SearchResults { get; set; } = new List<ProductSearchResult>();
        }

        public class ProductSearchResult
        {
            public int Id { get; set; }
            public string? Title { get; set; }
            public string? Description { get; set; }
            public string? Category { get; set; }
            public int Stock { get; set; }
        }
        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly LgwDbContext context;
            public Handler(LgwDbContext context)
            {
                this.context = context;
            }
            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                if (request.MinStock > request.MaxStock)
                    throw new InvalidRequestException("Min stock value cannot be greater than Max stock value");

                var products = context.Products.Include(p => p.Category).Where(p => p.Category != null && p.StockQuantity >= p.Category.MinStockToLive);

                if (request.SearchKey != null)
                {
                    products = products.Where(p => p.Title.ToLower().Contains(request.SearchKey.ToLower())
                    || (p.Description != null && p.Description.ToLower().Contains(request.SearchKey.ToLower()))
                    || (p.Category != null && p.Category.Name.ToLower().Contains(request.SearchKey.ToLower())));
                }

                if (request.MinStock != null)
                    products = products.Where(p => p.StockQuantity >= request.MinStock);

                if (request.MaxStock != null)
                    products = products.Where(p => p.StockQuantity <= request.MaxStock);

                return new Response
                {
                    SearchResults = await products
                    .AsNoTracking()
                    .Skip(request.PageNum * request.PageItemCount)
                    .Take(request.PageItemCount).Select(p => new ProductSearchResult
                    {
                        Title = p.Title,
                        Description = p.Description,
                        Category = p.Category.Name,
                        Stock = p.StockQuantity,
                        Id = p.Id
                    }).ToListAsync()
                };
            }

        }
    }
}