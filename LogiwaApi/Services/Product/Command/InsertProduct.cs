using LogiwaApi.Data;
using LogiwaApi.Exceptions;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace LogiwaApi.Services.Product.Command
{
    public static class InsertProduct
    {

        public class Request : IRequest<Unit>
        {
            [Required(ErrorMessage = "Title can not be null or empty")]
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
                if (request.Title.Length > 200)
                    throw new InvalidProductTitleException("Title should be maximum 200 characters");
                var product = new Data.Entities.Product()
                {
                    CategoryId = request.CategoryId,
                    Description = request.Description,
                    StockQuantity = request.StockQuantity ?? 0,
                    Title = request.Title,
                };

                var category = await context.Categories.FindAsync(request.CategoryId);
                if (category == null)
                    throw new InvalidCategoryException("Invalid CategoryId");

                await context.Products.AddAsync(product);
                await context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }


        }
    }
}
