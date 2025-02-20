using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StoreAPI.DTO;
using StoreAPI.Models;

namespace StoreAPI.Services.Banners
{
    public class BannerService : IBannerService
    {
        private readonly StoreDbContext _context;
        private readonly IMapper _mapper;

        public BannerService(StoreDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task CreateBannerAsync(BannerDTO bannerDTO)
        {
            var banner = _mapper.Map<Banner>(bannerDTO);

            await _context.Banners.AddAsync(banner);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBannerByIdAsync(int id)
        {
            var bannerToDelete = await GetBannerByIdAsync(id);

            _context.Banners.Remove(bannerToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task EditBannerAsync(BannerDTO bannerDTO)
        {
            var banner = _mapper.Map<Banner>(bannerDTO);

            _context.Banners.Update(banner);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsByIdAsync(int id) => await _context.Banners.AnyAsync(a => a.Id == id);
        public async Task<Banner> GetBannerByIdAsync(int id) => await _context.Banners.FirstAsync(a => a.Id == id);

        public async Task<IEnumerable<Banner>> GetBannersAsync() => await _context.Banners.ToListAsync();

    }
}
