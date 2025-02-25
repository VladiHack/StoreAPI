using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StoreAPI.DTO;
using StoreAPI.Models;

namespace StoreAPI.Services.ProductTypes
{
    public class ProductTypeService : IProductTypeService
    {
        private readonly StoreDbContext _context;
        private readonly IMapper _mapper;

        public ProductTypeService(StoreDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task CreateProductTypeAsync(ProductTypeDTO productTypeDTO)
        {
            var productType = _mapper.Map<ProductType>(productTypeDTO);
            await _context.ProductTypes.AddAsync(productType);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductTypeByIdAsync(int id)
        {
            var productTypeToDelete = await GetProductTypeByIdAsync(id);
            _context.ProductTypes.Remove(productTypeToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task EditProductTypeAsync(ProductTypeDTO productTypeDTO)
        {
            var productType = _mapper.Map<ProductType>(productTypeDTO);
            _context.ProductTypes.Update(productType);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsByIdAsync(int id) => await _context.ProductTypes.AnyAsync(pt => pt.Id == id);

        public async Task<ProductType> GetProductTypeByIdAsync(int id) => await _context.ProductTypes.FirstAsync(pt => pt.Id == id);

        public async Task<IEnumerable<ProductType>> GetProductTypesAsync() => await _context.ProductTypes.ToListAsync();
    }
}
