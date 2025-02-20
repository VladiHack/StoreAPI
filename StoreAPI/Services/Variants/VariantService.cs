using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StoreAPI.DTO;
using StoreAPI.Models;

namespace StoreAPI.Services.Variants
{
    public class VariantService : IVariantService
    {
        private readonly StoreDbContext _context;
        private readonly IMapper _mapper;

        public VariantService(StoreDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task CreateVariantAsync(VariantDTO variantDTO)
        {
            var variant = _mapper.Map<Variant>(variantDTO);
            await _context.Variants.AddAsync(variant);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteVariantByIdAsync(int id)
        {
            var variantToDelete = await GetVariantByIdAsync(id);
            _context.Variants.Remove(variantToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task EditVariantAsync(VariantDTO variantDTO)
        {
            var variant = _mapper.Map<Variant>(variantDTO);
            _context.Variants.Update(variant);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsByIdAsync(int id)
            => await _context.Variants.AnyAsync(a => a.Id == id);

        public async Task<Variant> GetVariantByIdAsync(int id)
            => await _context.Variants.FirstAsync(a => a.Id == id);

        public async Task<IEnumerable<Variant>> GetVariantsAsync()
            => await _context.Variants.ToListAsync();
    }
}
