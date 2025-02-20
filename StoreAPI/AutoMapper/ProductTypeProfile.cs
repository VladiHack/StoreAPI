using AutoMapper;
using StoreAPI.DTO;
using StoreAPI.Models;

namespace StoreAPI.AutoMapper
{
    public class ProductTypeProfile:Profile
    {
        public ProductTypeProfile()
        {
            CreateMap<ProductTypeDTO, ProductType>();
        }
    }
}
