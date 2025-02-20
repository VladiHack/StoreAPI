using StoreAPI.DTO;
using StoreAPI.Models;

namespace StoreAPI.Services.Collections
{
    public interface ICollectionService
    {
        Task<IEnumerable<Collection>> GetCollectionsAsync();
        Task CreateCollectionAsync(CollectionDTO collectionDTO);
        Task DeleteCollectionByIdAsync(int id);
        Task EditCollectionAsync(CollectionDTO collectionDTO);
        Task<Collection> GetCollectionByIdAsync(int id);
        Task<bool> ExistsByIdAsync(int id);
    }
}
