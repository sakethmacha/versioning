using ApiVersioning.DbContexts;
using ApiVersioning.Dtos;
using ApiVersioning.Interfaces;
using ApiVersioning.Models;
using Microsoft.EntityFrameworkCore;
namespace ApiVersioning.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _db;

        public ProductService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Product> CreateProductAsync(
            CreateProductRequest request)
        {
            // Optional: validate category exists
            var category = await _db.Categories
                .FirstOrDefaultAsync(c => c.Id == request.CategoryId);

            if (category == null)
                throw new InvalidOperationException(
                    $"Category with id {request.CategoryId} not found");

            var product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Category = category,
                Tags = string.IsNullOrWhiteSpace(request.Tags)
            ? null
            : request.Tags,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "system"
            };

            _db.Products.Add(product);
            await _db.SaveChangesAsync();

            return product;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            var product = await _db.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                throw new KeyNotFoundException(
                    $"Product with id {id} not found");

            return product;
        }
    }
}

