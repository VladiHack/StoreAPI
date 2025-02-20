using AutoMapper;
using StoreAPI.DTO;
using StoreAPI.Models;

namespace StoreAPI.AutoMapper
{
    public class CollectionProductProfile:Profile
    {
        public CollectionProductProfile()
        {
            CreateMap<CollectionProductDTO, CollectionProduct>();
        }
    }
}
