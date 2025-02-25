using StoreAPI.DTO;
using StoreAPI.Models;

namespace StoreAPI.Services.ProductTypes
{
    public interface IProductTypeService
    {
        Task<IEnumerable<ProductType>> GetProductTypesAsync();
        Task CreateProductTypeAsync(ProductTypeDTO productTypeDTO);
        Task DeleteProductTypeByIdAsync(int id);
        Task EditProductTypeAsync(ProductTypeDTO productTypeDTO);
        Task<ProductType> GetProductTypeByIdAsync(int id);
        Task<bool> ExistsByIdAsync(int id);
    }
}
