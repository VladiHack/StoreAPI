using AutoMapper;
using StoreAPI.DTO;
using StoreAPI.Models;

namespace StoreAPI.AutoMapper
{
    public class StoreOptionProfile:Profile
    {
        public StoreOptionProfile()
        {
            CreateMap<StoreOptionDTO, StoreOption>();
        }

    }
}
