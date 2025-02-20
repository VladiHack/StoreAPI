using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StoreAPI.DTO;
using StoreAPI.Models;

namespace StoreAPI.Services.ProductVariants
{
    public class ProductVariantService : IProductVariantService
    {
        private readonly StoreDbContext _context;
        private readonly IMapper _mapper;

        public ProductVariantService(StoreDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task CreateProductVariantAsync(ProductVariantDTO productVariantDTO)
        {
            var productVariant = _mapper.Map<ProductVariant>(productVariantDTO);
            await _context.ProductVariants.AddAsync(productVariant);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductVariantByIdAsync(int id)
        {
            var productVariantToDelete = await GetProductVariantByIdAsync(id);
            _context.ProductVariants.Remove(productVariantToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task EditProductVariantAsync(ProductVariantDTO productVariantDTO)
        {
            var productVariant = _mapper.Map<ProductVariant>(productVariantDTO);
            _context.ProductVariants.Update(productVariant);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsByIdAsync(int id) => await _context.ProductVariants.AnyAsync(a => a.Id == id);

        public async Task<ProductVariant> GetProductVariantByIdAsync(int id) => await _context.ProductVariants.FirstAsync(a => a.Id == id);

        public async Task<IEnumerable<ProductVariant>> GetProductVariantsAsync() => await _context.ProductVariants.ToListAsync();
    }
}
