using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StoreAPI.DTO;
using StoreAPI.Models;

namespace StoreAPI.Services.CollectionProducts
{
    public class CollectionProductService : ICollectionProductService
    {
        private readonly StoreDbContext _context;
        private readonly IMapper _mapper;

        public CollectionProductService(StoreDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task CreateCollectionProductAsync(CollectionProductDTO collectionProductDTO)
        {
            var collectionProduct = _mapper.Map<CollectionProduct>(collectionProductDTO);
            await _context.CollectionProducts.AddAsync(collectionProduct);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCollectionProductByIdAsync(int id)
        {
            var collectionProductToDelete = await GetCollectionProductByIdAsync(id);
            _context.CollectionProducts.Remove(collectionProductToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task EditCollectionProductAsync(CollectionProductDTO collectionProductDTO)
        {
            var collectionProduct = _mapper.Map<CollectionProduct>(collectionProductDTO);
            _context.CollectionProducts.Update(collectionProduct);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsByIdAsync(int id) => await _context.CollectionProducts.AnyAsync(a => a.Id == id);

        public async Task<CollectionProduct> GetCollectionProductByIdAsync(int id) => await _context.CollectionProducts.FirstAsync(a => a.Id == id);

        public async Task<IEnumerable<CollectionProduct>> GetCollectionProductsAsync() => await _context.CollectionProducts.ToListAsync();
    }
}
