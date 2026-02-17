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
        
        private readonly IProductService ProductService;

        public ProductsController(IProductService productService)
        {
            ProductService = productService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await ProductService.CreateProductAsync(request);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await ProductService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }
    }
}
