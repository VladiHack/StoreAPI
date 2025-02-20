using AutoMapper;
using StoreAPI.DTO;
using StoreAPI.Models;

namespace StoreAPI.AutoMapper
{
    public class ProductProfile:Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductDTO, Product>();
        }
    }
}
