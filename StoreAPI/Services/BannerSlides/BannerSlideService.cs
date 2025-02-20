using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StoreAPI.DTO;
using StoreAPI.Models;

namespace StoreAPI.Services.BannerSlides
{
    public class BannerSlideService : IBannerSlideService
    {
        private readonly StoreDbContext _context;
        private readonly IMapper _mapper;

        public BannerSlideService(StoreDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task CreateBannerSlideAsync(BannerSlideDTO bannerSlideDTO)
        {
            var bannerSlide = _mapper.Map<BannerSlide>(bannerSlideDTO);
            await _context.BannerSlides.AddAsync(bannerSlide);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBannerSlideByIdAsync(int id)
        {
            var bannerSlideToDelete = await GetBannerSlideByIdAsync(id);
            _context.BannerSlides.Remove(bannerSlideToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task EditBannerSlideAsync(BannerSlideDTO bannerSlideDTO)
        {
            var bannerSlide = _mapper.Map<BannerSlide>(bannerSlideDTO);
            _context.BannerSlides.Update(bannerSlide);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsByIdAsync(int id) => await _context.BannerSlides.AnyAsync(a => a.Id == id);
        public async Task<BannerSlide> GetBannerSlideByIdAsync(int id) => await _context.BannerSlides.FirstAsync(a => a.Id == id);
        public async Task<IEnumerable<BannerSlide>> GetBannerSlidesAsync() => await _context.BannerSlides.ToListAsync();
    }
}
