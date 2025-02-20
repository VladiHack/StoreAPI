using StoreAPI.DTO;
using StoreAPI.Models;

namespace StoreAPI.Services.Products
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetProductsAsync();
        Task CreateProductAsync(ProductDTO productDTO);
        Task DeleteProductByIdAsync(int id);
        Task EditProductAsync(ProductDTO productDTO);
        Task<Product> GetProductByIdAsync(int id);
        Task<bool> ExistsByIdAsync(int id);
    }
}
