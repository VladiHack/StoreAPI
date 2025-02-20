using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StoreAPI.DTO;
using StoreAPI.Models;

namespace StoreAPI.Services.StoreOptions
{
    public class StoreOptionService : IStoreOptionService
    {
        private readonly StoreDbContext _context;
        private readonly IMapper _mapper;

        public StoreOptionService(StoreDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task CreateStoreOptionAsync(StoreOptionDTO storeOptionDTO)
        {
            var storeOption = _mapper.Map<StoreOption>(storeOptionDTO);
            await _context.StoreOptions.AddAsync(storeOption);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteStoreOptionByIdAsync(int id)
        {
            var storeOptionToDelete = await GetStoreOptionByIdAsync(id);
            _context.StoreOptions.Remove(storeOptionToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task EditStoreOptionAsync(StoreOptionDTO storeOptionDTO)
        {
            var storeOption = _mapper.Map<StoreOption>(storeOptionDTO);
            _context.StoreOptions.Update(storeOption);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsByIdAsync(int id) => await _context.StoreOptions.AnyAsync(a => a.Id == id);

        public async Task<StoreOption> GetStoreOptionByIdAsync(int id) => await _context.StoreOptions.FirstAsync(a => a.Id == id);

        public async Task<IEnumerable<StoreOption>> GetStoreOptionsAsync() => await _context.StoreOptions.ToListAsync();
    }
}
