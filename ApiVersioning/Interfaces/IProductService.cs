using ApiVersioning.Dtos;
using ApiVersioning.Models;

namespace ApiVersioning.Interfaces
{
    public interface IProductService
    {
        Task<Product> CreateProductAsync(CreateProductRequest request);
        Task<Product> GetProductByIdAsync(int id);
    }
}
