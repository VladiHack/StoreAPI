using StoreAPI.DTO;
using StoreAPI.Models;

namespace StoreAPI.Services.ProductImages
{
    public interface IProductImageService
    {
        Task<IEnumerable<ProductImage>> GetProductImagesAsync();
        Task CreateProductImageAsync(ProductImageDTO productImageDTO);
        Task DeleteProductImageByIdAsync(int id);
        Task EditProductImageAsync(ProductImageDTO productImageDTO);
        Task<ProductImage> GetProductImageByIdAsync(int id);
        Task<bool> ExistsByIdAsync(int id);
    }
}
