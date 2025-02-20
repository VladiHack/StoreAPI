using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StoreAPI.DTO;
using StoreAPI.Models;

namespace StoreAPI.Services.ProductImages
{
    public class ProductImageService : IProductImageService
    {
        private readonly StoreDbContext _context;
        private readonly IMapper _mapper;

        public ProductImageService(StoreDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task CreateProductImageAsync(ProductImageDTO productImageDTO)
        {
            var productImage = _mapper.Map<ProductImage>(productImageDTO);
            await _context.ProductImages.AddAsync(productImage);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductImageByIdAsync(int id)
        {
            var productImageToDelete = await GetProductImageByIdAsync(id);
            _context.ProductImages.Remove(productImageToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task EditProductImageAsync(ProductImageDTO productImageDTO)
        {
            var productImage = _mapper.Map<ProductImage>(productImageDTO);
            _context.ProductImages.Update(productImage);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsByIdAsync(int id) => await _context.ProductImages.AnyAsync(a => a.Id == id);

        public async Task<ProductImage> GetProductImageByIdAsync(int id) => await _context.ProductImages.FirstAsync(a => a.Id == id);

        public async Task<IEnumerable<ProductImage>> GetProductImagesAsync() => await _context.ProductImages.ToListAsync();
    }
}
