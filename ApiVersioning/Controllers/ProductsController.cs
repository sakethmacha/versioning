using ApiVersioning.Dtos;
using ApiVersioning.Interfaces;
using Microsoft.AspNetCore.Mvc;
namespace ApiVersioning.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v1/products")]
    public class ProductsController : ControllerBase
    {
        
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _productService.CreateProductAsync(request);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }
    }
}
