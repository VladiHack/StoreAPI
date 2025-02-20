using StoreAPI.DTO;
using StoreAPI.Models;

namespace StoreAPI.Services.BannerSlides
{
    public interface IBannerSlideService
    {
        Task<IEnumerable<BannerSlide>> GetBannerSlidesAsync();
        Task CreateBannerSlideAsync(BannerSlideDTO bannerSlideDTO);
        Task DeleteBannerSlideByIdAsync(int id);
        Task EditBannerSlideAsync(BannerSlideDTO bannerSlideDTO);
        Task<BannerSlide> GetBannerSlideByIdAsync(int id);
        Task<bool> ExistsByIdAsync(int id);
    }
}
