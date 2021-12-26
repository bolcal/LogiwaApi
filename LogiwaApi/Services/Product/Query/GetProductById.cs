using LogiwaApi.Data;
using LogiwaApi.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LogiwaApi.Services.Product.Query
{
    public static class GetProductById
    {
        public class Request : IRequest<Response>
        {
            public int ProductId { get; set; }
        }
        public class Response
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
                var product = await context.Products.Include(p => p.Category).Where(p => p.Id == request.ProductId).FirstOrDefaultAsync();
                if (product == null)
                {
                    throw new EntityNotFoundException($"No product found with id {request.ProductId}");
                }

                return new Response
                {
                    Category = product.Category?.Name,
                    Description = product.Description ?? String.Empty,
                    Id = product.Id,
                    Stock = product.StockQuantity,
                    Title = product.Title
                };

            }
        }

    }
}
