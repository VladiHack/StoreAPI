using StoreAPI.DTO;
using StoreAPI.Models;

namespace StoreAPI.Services.Variants
{
    public interface IVariantService
    {
        Task<IEnumerable<Variant>> GetVariantsAsync();
        Task CreateVariantAsync(VariantDTO variantDTO);
        Task DeleteVariantByIdAsync(int id);
        Task EditVariantAsync(VariantDTO variantDTO);
        Task<Variant> GetVariantByIdAsync(int id);
        Task<bool> ExistsByIdAsync(int id);
    }
}
