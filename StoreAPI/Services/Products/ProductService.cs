using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StoreAPI.DTO;
using StoreAPI.Models;

namespace StoreAPI.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly StoreDbContext _context;
        private readonly IMapper _mapper;

        public ProductService(StoreDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task CreateProductAsync(ProductDTO productDTO)
        {
            var product = _mapper.Map<Product>(productDTO);
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductByIdAsync(int id)
        {
            var productToDelete = await GetProductByIdAsync(id);
            _context.Products.Remove(productToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task EditProductAsync(ProductDTO productDTO)
        {
            var product = _mapper.Map<Product>(productDTO);
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsByIdAsync(int id) => await _context.Products.AnyAsync(a => a.Id == id);
        public async Task<Product> GetProductByIdAsync(int id) => await _context.Products.FirstAsync(a => a.Id == id);
        public async Task<IEnumerable<Product>> GetProductsAsync() => await _context.Products.ToListAsync();
    }
}
