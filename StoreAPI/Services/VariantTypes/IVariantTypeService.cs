using StoreAPI.DTO;
using StoreAPI.Models;

namespace StoreAPI.Services.VariantTypes
{
    public interface IVariantTypeService
    {
        Task<IEnumerable<VariantType>> GetVariantTypesAsync();
        Task CreateVariantTypeAsync(VariantTypeDTO variantTypeDTO);
        Task DeleteVariantTypeByIdAsync(int id);
        Task EditVariantTypeAsync(VariantTypeDTO variantTypeDTO);
        Task<VariantType> GetVariantTypeByIdAsync(int id);
        Task<bool> ExistsByIdAsync(int id);
    }
}
