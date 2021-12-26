using LogiwaApi.Data;
using LogiwaApi.Exceptions;
using MediatR;

namespace LogiwaApi.Services.Product.Command
{
    public static class DeleteProduct
    {
        public class Request : IRequest
        {
            public int Id { get; set; }
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
                    throw new EntityNotFoundException($"No Product with Id : {request.Id}");
                context.Products.Remove(product);
                await context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}
