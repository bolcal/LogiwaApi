using LogiwaApi.Services.Product.Command;
using LogiwaApi.Services.Product.Query;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LogiwaApi.Services.Product
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMediator mediator;
        public ProductController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetProductById.Response))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SearchProduct.Response>> GetProducts([FromBody] SearchProduct.Request request, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(request, cancellationToken);
            return Ok(result);
        }

        [HttpGet("{ProductId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetProductById.Response))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetProductById.Response>> GetProductsById([FromRoute] GetProductById.Request request, CancellationToken cancellationToken)
        {
            return Ok(await mediator.Send(request, cancellationToken));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task Update([FromBody] UpdateProduct.Request request, CancellationToken cancellationToken)
        {
            await mediator.Send(request, cancellationToken);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Insert([FromBody] InsertProduct.Request request, CancellationToken cancellationToken)
        {
            return Ok(await mediator.Send(request, cancellationToken));
        }

        [HttpDelete("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task Delete([FromRoute] DeleteProduct.Request request, CancellationToken cancellationToken)
        {
            await mediator.Send(request, cancellationToken);
        }

    }
}
