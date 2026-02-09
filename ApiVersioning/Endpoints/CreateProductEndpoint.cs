using ApiVersioning.Dtos;
using FastEndpoints;
using ApiVersioning.Interfaces;
using ApiVersioning.Validations;
namespace ApiVersioning.Endpoints
{
    public class CreateProductEndpoint : Endpoint<CreateProductRequest, ProductResponse>
    {
        private readonly IProductService _productService;
        private readonly ILogger<CreateProductEndpoint> _logger;

        public CreateProductEndpoint(IProductService productService, ILogger<CreateProductEndpoint> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        public override void Configure()
        {
            Post("/product");
            Version(2);
            Tags("Product");
            // Authentication & Authorization
            AllowAnonymous(); // Change to: Roles("Admin", "User") for role-based auth

            // Validation
            Validator<CreateProductValidator>();

            // API Documentation
            Description(b => b
                .Accepts<CreateProductRequest>("application/json")
                .Produces<ProductResponse>(200, "application/json")
                .ProducesProblemDetails(400, "application/problem+json")
                .ProducesProblemDetails(401, "application/problem+json")
                .ProducesProblemDetails(429, "application/problem+json"));

            Summary(s =>
            {
                s.Summary = "Create a new product";
                s.Description = "Creates a new product with the provided details";
                s.ExampleRequest = new CreateProductRequest
                {
                    Name = "Gaming Laptop",
                    Description = "High-performance gaming laptop with RTX 4090",
                    Price = 2499.99m,
                    CategoryId = 1,
                    Tags = "electronics,gaming,laptop"
                };
            });


            // Versioning
            //Options(x => x.WithVersionSet("v2"));


            // Throttling
            Throttle(hitLimit: 10, durationSeconds: 60, headerName: "X-Rate-Limit");
        }

        public override async Task HandleAsync(CreateProductRequest req, CancellationToken ct)
        {
            // Track request start time for postprocessor
            HttpContext.Items["RequestStartTime"] = DateTime.UtcNow;

            try
            {
                _logger.LogInformation("Creating product: {ProductName}", req.Name);

                // Business logic
                var product = await _productService.CreateProductAsync(req);

                // Map to response
                var response = new ProductResponse
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    CategoryName = product.Category?.Name ?? "Unknown",
                    Tags = product.Tags,
                    CreatedAt = product.CreatedAt,
                    CreatedBy = HttpContext.Items["UserId"]?.ToString() ?? "System",
                    IsDeleted = product.IsDeleted
                };

                _logger.LogInformation("Product created successfully: {ProductId}", response.Id);

                // Send response
                await SendAsync(response, 201, ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating product: {ProductName}", req.Name);
                await SendErrorsAsync(500, ct);
            }
        }
    }

}
