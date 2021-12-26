using LogiwaApi.Data;
using LogiwaApi.Exceptions;
using MediatR;

namespace LogiwaApi.Services.Product.Command
{
    public static class UpdateProduct
    {
        public class Request : IRequest<Unit>
        {
            public int Id { get; set; }
            public string? Title { get; set; }
            public string? Description { get; set; }
            public int? CategoryId { get; set; }
            public int? StockQuantity { get; set; }
        }

        public class Handler : IRequestHandler<Request, Unit>
        {
            private readonly LgwDbContext context;
            public Handler(LgwDbContext context)
            {
                this.context = context;
            }

            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                var product = await context.Products.FindAsync(request.Id);
                if (product == null)
                    throw new EntityNotFoundException($"No product found with id {request.Id} to update");

                if (request.Title != null && request.Title.Trim() == String.Empty)
                    throw new InvalidProductTitleException("Title can not be empty");

                if (request.Title != null && request.Title.Length > 200)
                    throw new InvalidProductTitleException("Title should be maximum 200 characters");

                product.Title = request.Title ?? product.Title;
                product.Description = request.Description ?? product.Description;
                product.CategoryId = request.CategoryId ?? product.CategoryId;
                product.StockQuantity = request.StockQuantity ?? product.StockQuantity;
                await context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}
