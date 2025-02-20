using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StoreAPI.DTO;
using StoreAPI.Models;

namespace StoreAPI.Services.Users
{
    public class UserService : IUserService
    {
        private readonly StoreDbContext _context;
        private readonly IMapper _mapper;

        public UserService(StoreDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task CreateUserAsync(UserDTO userDTO)
        {
            var user = _mapper.Map<User>(userDTO);
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserByIdAsync(int id)
        {
            var userToDelete = await GetUserByIdAsync(id);
            _context.Users.Remove(userToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task EditUserAsync(UserDTO userDTO)
        {
            var user = _mapper.Map<User>(userDTO);
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsByIdAsync(int id) => await _context.Users.AnyAsync(a => a.Id == id);

        public async Task<User> GetUserByIdAsync(int id) => await _context.Users.FirstAsync(a => a.Id == id);

        public async Task<IEnumerable<User>> GetUsersAsync() => await _context.Users.ToListAsync();
    }
}
