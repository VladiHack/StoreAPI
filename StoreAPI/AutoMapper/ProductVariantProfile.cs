using AutoMapper;
using StoreAPI.DTO;
using StoreAPI.Models;

namespace StoreAPI.AutoMapper
{
    public class ProductVariantProfile:Profile
    {
        public ProductVariantProfile()
        {
            CreateMap<ProductVariantDTO, ProductVariant>();
        }
    }
}
