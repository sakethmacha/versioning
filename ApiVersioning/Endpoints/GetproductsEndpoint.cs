using ApiVersioning.Dtos;
using ApiVersioning.Interfaces;
using ApiVersioning.Validations;
using FastEndpoints;

namespace ApiVersioning.Endpoints
{
    public class GetProductEndpoint : Endpoint<GetProductRequest, ProductResponse>
    {
        private readonly IProductService _productService;
        private readonly ILogger<GetProductEndpoint> _logger;

        public GetProductEndpoint(IProductService productService, ILogger<GetProductEndpoint> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        public override void Configure()
        {
            Get("/product/{id:int}");
            Version(2);
            AllowAnonymous();
            Tags("Product");
            // Validation
            Validator<GetProductValidator>();

            Description(b => b
                .Produces<ProductResponse>(200)
                .ProducesProblemDetails(404));

            Summary(s =>
            {
                s.Summary = "Get product by ID";
                s.Description = "Retrieves a product by its unique identifier";
            });
        }

        public override async Task HandleAsync(GetProductRequest req, CancellationToken ct)
        {
            try
            {
                _logger.LogInformation("Fetching product: {ProductId}", req.Id);

                var product = await _productService.GetProductByIdAsync(req.Id);

                if (product == null)
                {
                    await SendNotFoundAsync(ct);
                    return;
                }

                var response = new ProductResponse
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    CategoryName = product.Category?.Name ?? "Unknown",
                    Tags = product.Tags,
                    CreatedAt = product.CreatedAt,
                    CreatedBy = product.CreatedBy,
                    IsActive = product.IsActive
                };

                await SendAsync(response, 200, ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching product: {ProductId}", req.Id);
                await SendErrorsAsync(500, ct);
            }
        }
    }
}
