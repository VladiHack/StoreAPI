using StoreAPI.DTO;
using StoreAPI.Models;

namespace StoreAPI.Services.Banners
{
    public interface IBannerService
    {
        Task<IEnumerable<Banner>> GetBannersAsync();
        Task CreateBannerAsync(BannerDTO bannerDTO);
        Task DeleteBannerByIdAsync(int id);
        Task EditBannerAsync(BannerDTO bannerDTO);

        Task<Banner> GetBannerByIdAsync(int id);

        Task<bool> ExistsByIdAsync(int id);
    }
}
