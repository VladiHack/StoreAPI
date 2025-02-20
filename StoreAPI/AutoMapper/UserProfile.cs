using AutoMapper;
using StoreAPI.DTO;
using StoreAPI.Models;

namespace StoreAPI.AutoMapper
{
    public class UserProfile:Profile
    {
        public UserProfile() 
        {
            CreateMap<UserDTO, User>();
        }
    }
}
