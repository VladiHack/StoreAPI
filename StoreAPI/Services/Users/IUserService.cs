using StoreAPI.DTO;
using StoreAPI.Models;

namespace StoreAPI.Services.Users
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetUsersAsync();
        Task CreateUserAsync(UserDTO userDTO);
        Task DeleteUserByIdAsync(int id);
        Task EditUserAsync(UserDTO userDTO);
        Task<User> GetUserByIdAsync(int id);
        Task<bool> ExistsByIdAsync(int id);
    }
}
