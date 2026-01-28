using Microsoft.AspNetCore.Mvc;
using ApiVersioning.Dtos;
using ApiVersioning.Models;
namespace ApiVersioning.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<ProductDto>> Get(
            [FromQuery] ProductFilterQuery filter)
        {
            var products = new List<Product>
        {
            new() { Id = 1, Name = "Book", Price = 200, Category = "Education" },
            new() { Id = 2, Name = "Pen", Price = 20, Category = "Stationery" },
            new() { Id = 3, Name = "Laptop", Price = 50000, Category = "Electronics" },
            new() { Id = 4, Name = "Notebook", Price = 100, Category = "Stationery" }
        };

            // FILTERING
            if (!string.IsNullOrWhiteSpace(filter.Name))
                products = products
                    .Where(p => p.Name.Contains(filter.Name,
                        StringComparison.OrdinalIgnoreCase))
                    .ToList();

            if (filter.MinPrice.HasValue)
                products = products
                    .Where(p => p.Price >= filter.MinPrice.Value)
                    .ToList();

            if (filter.MaxPrice.HasValue)
                products = products
                    .Where(p => p.Price <= filter.MaxPrice.Value)
                    .ToList();

            if (!string.IsNullOrWhiteSpace(filter.Category))
                products = products
                    .Where(p => p.Category == filter.Category)
                    .ToList();

            var result = products.Select(p => new ProductDto
            {
                Name = p.Name,
                Price = p.Price,
                Category = p.Category
            }).ToList();

            return Ok(result);
        }

    }
}
