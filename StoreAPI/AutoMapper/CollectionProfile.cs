using AutoMapper;
using StoreAPI.DTO;
using StoreAPI.Models;

namespace StoreAPI.AutoMapper
{
    public class CollectionProfile:Profile
    {
        public CollectionProfile()
        {
            CreateMap<CollectionDTO, Collection>();
        }
    }
}
