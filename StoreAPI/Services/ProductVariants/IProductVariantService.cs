using StoreAPI.DTO;
using StoreAPI.Models;

namespace StoreAPI.Services.ProductVariants
{
    public interface IProductVariantService
    {
        Task<IEnumerable<ProductVariant>> GetProductVariantsAsync();
        Task CreateProductVariantAsync(ProductVariantDTO productVariantDTO);
        Task DeleteProductVariantByIdAsync(int id);
        Task EditProductVariantAsync(ProductVariantDTO productVariantDTO);
        Task<ProductVariant> GetProductVariantByIdAsync(int id);
        Task<bool> ExistsByIdAsync(int id);
    }
}
