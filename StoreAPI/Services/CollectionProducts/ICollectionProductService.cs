using StoreAPI.DTO;
using StoreAPI.Models;

namespace StoreAPI.Services.CollectionProducts
{
    public interface ICollectionProductService
    {
        Task<IEnumerable<CollectionProduct>> GetCollectionProductsAsync();
        Task CreateCollectionProductAsync(CollectionProductDTO collectionProductDTO);
        Task DeleteCollectionProductByIdAsync(int id);
        Task EditCollectionProductAsync(CollectionProductDTO collectionProductDTO);
        Task<CollectionProduct> GetCollectionProductByIdAsync(int id);
        Task<bool> ExistsByIdAsync(int id);
    }
}
