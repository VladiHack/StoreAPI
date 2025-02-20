using AutoMapper;
using StoreAPI.DTO;
using StoreAPI.Models;

namespace StoreAPI.AutoMapper
{
    public class VariantProfile:Profile
    {
        public VariantProfile() 
        {
            CreateMap<VariantDTO, VariantProfile>();
        }
    }
}
