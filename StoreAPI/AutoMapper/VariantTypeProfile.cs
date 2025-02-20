using AutoMapper;
using StoreAPI.DTO;
using StoreAPI.Models;

namespace StoreAPI.AutoMapper
{
    public class VariantTypeProfile:Profile
    {
        public VariantTypeProfile() 
        {
            CreateMap<VariantTypeDTO, VariantType>();
        }
    }
}
