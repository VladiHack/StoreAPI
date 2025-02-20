using AutoMapper;
using StoreAPI.DTO;
using StoreAPI.Models;

namespace StoreAPI.AutoMapper
{
    public class ProductImageProfile:Profile
    {
        public ProductImageProfile()
        {
            CreateMap<ProductImageDTO, ProductImage>();
        }
    }
}
