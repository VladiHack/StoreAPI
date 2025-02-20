using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StoreAPI.DTO;
using StoreAPI.Models;

namespace StoreAPI.Services.Collections
{
    public class CollectionService : ICollectionService
    {
        private readonly StoreDbContext _context;
        private readonly IMapper _mapper;

        public CollectionService(StoreDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task CreateCollectionAsync(CollectionDTO collectionDTO)
        {
            var collection = _mapper.Map<Collection>(collectionDTO);
            await _context.Collections.AddAsync(collection);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCollectionByIdAsync(int id)
        {
            var collectionToDelete = await GetCollectionByIdAsync(id);
            _context.Collections.Remove(collectionToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task EditCollectionAsync(CollectionDTO collectionDTO)
        {
            var collection = _mapper.Map<Collection>(collectionDTO);
            _context.Collections.Update(collection);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsByIdAsync(int id) => await _context.Collections.AnyAsync(a => a.Id == id);
        public async Task<Collection> GetCollectionByIdAsync(int id) => await _context.Collections.FirstAsync(a => a.Id == id);
        public async Task<IEnumerable<Collection>> GetCollectionsAsync() => await _context.Collections.ToListAsync();
    }
}
