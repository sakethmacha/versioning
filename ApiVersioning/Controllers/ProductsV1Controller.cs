using ApiVersioning.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiVersioning.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/products")]
    public class ProductsV1Controller : ControllerBase
    {
        [HttpGet]
        public ActionResult<ProductV1Dto> Get()
        {
            return Ok(new ProductV1Dto
            {
                Name = "Book"
            });
        }
    }

}
