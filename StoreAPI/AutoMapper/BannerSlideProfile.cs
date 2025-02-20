using AutoMapper;
using StoreAPI.DTO;
using StoreAPI.Models;

namespace StoreAPI.AutoMapper
{
    public class BannerSlideProfile:Profile
    {
        public BannerSlideProfile()
        {
            CreateMap<BannerSlideDTO, BannerSlide>();
        }
    }
}
