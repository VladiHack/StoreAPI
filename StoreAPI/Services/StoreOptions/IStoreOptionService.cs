using StoreAPI.DTO;
using StoreAPI.Models;

namespace StoreAPI.Services.StoreOptions
{
    public interface IStoreOptionService
    {
        Task<IEnumerable<StoreOption>> GetStoreOptionsAsync();
        Task CreateStoreOptionAsync(StoreOptionDTO storeOptionDTO);
        Task DeleteStoreOptionByIdAsync(int id);
        Task EditStoreOptionAsync(StoreOptionDTO storeOptionDTO);
        Task<StoreOption> GetStoreOptionByIdAsync(int id);
        Task<bool> ExistsByIdAsync(int id);
    }
}
