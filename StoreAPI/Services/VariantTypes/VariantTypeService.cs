using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StoreAPI.DTO;
using StoreAPI.Models;

namespace StoreAPI.Services.VariantTypes
{
    public class VariantTypeService : IVariantTypeService
    {
        private readonly StoreDbContext _context;
        private readonly IMapper _mapper;

        public VariantTypeService(StoreDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task CreateVariantTypeAsync(VariantTypeDTO variantTypeDTO)
        {
            var variantType = _mapper.Map<VariantType>(variantTypeDTO);
            await _context.VariantTypes.AddAsync(variantType);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteVariantTypeByIdAsync(int id)
        {
            var variantTypeToDelete = await GetVariantTypeByIdAsync(id);
            _context.VariantTypes.Remove(variantTypeToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task EditVariantTypeAsync(VariantTypeDTO variantTypeDTO)
        {
            var variantType = _mapper.Map<VariantType>(variantTypeDTO);
            _context.VariantTypes.Update(variantType);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsByIdAsync(int id)
            => await _context.VariantTypes.AnyAsync(a => a.Id == id);

        public async Task<VariantType> GetVariantTypeByIdAsync(int id)
            => await _context.VariantTypes.FirstAsync(a => a.Id == id);

        public async Task<IEnumerable<VariantType>> GetVariantTypesAsync()
            => await _context.VariantTypes.ToListAsync();
    }
}
