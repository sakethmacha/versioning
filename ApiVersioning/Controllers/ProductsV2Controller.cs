using ApiVersioning.Dtos;
using Microsoft.AspNetCore.Mvc;
using ApiVersioning.Pagination;
namespace ApiVersioning.Controllers
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/products")]
    public class ProductsV2Controller : ControllerBase
    {
        [HttpGet]
        public ActionResult<PagedResponse<ProductV2Dto>> Get([FromQuery] PaginationQuery pagination)
        {
            var allProducts = Enumerable.Range(1, 50).Select(i =>
                new ProductV2Dto
                {
                    Name = $"Product {i}",
                    Price = 10 + i
                }).ToList();

            var pagedData = allProducts
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToList();

            return Ok(new PagedResponse<ProductV2Dto>
            {
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize,
                TotalRecords = allProducts.Count,
                Data = pagedData
            });
        }
    }
}
