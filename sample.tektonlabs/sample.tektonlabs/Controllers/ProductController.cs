using MediatR;
using Microsoft.AspNetCore.Mvc;
using sample.tektonlabs.core.Handlers;
using sample.tektonlabs.core.Requests;

namespace sample.tektonlabs.Controllers
{
    [ApiController]
    [Route("")]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{key}")]
        public async Task<IActionResult> Get([FromRoute] string key)
        {
            var product = await _mediator.Send(new GetProductByKeyRequest { Key = key });
            return Ok(product);
        }
        [HttpPost()]
        public async Task<IActionResult> Insert(InsertProductRequest request)
        {
            var product = await _mediator.Send(request);
            return Ok(product);
        }
        [HttpPut()]
        public async Task<IActionResult> Update(UpdateProductRequest request)
        {
            var product = await _mediator.Send(request);
            return Ok(product);
        }
    }
}