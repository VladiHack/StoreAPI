using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreAPI.DTO;
using StoreAPI.Models;

namespace StoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly StoreDbContext _context;
        private readonly IMapper _mapper;

        public UserController(StoreDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/User
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<User>>> GetUsersAsync()
        {
            return Ok(await _context.Users.ToListAsync());
        }

        // GET: api/User/{id}
        [HttpGet("{id:int}", Name = "GetUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<User>> GetUserAsync(int id)
        {
            if (id < 0)
            {
                return BadRequest("Invalid ID.");
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            return Ok(user);
        }

        // POST: api/User
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<User>> CreateUserAsync(
            [FromBody] UserDTO userDTO)
        {
            if (userDTO == null)
            {
                return BadRequest("UserDTO cannot be null.");
            }

            if (userDTO.Id > 0)
            {
                return BadRequest("Id should not be provided for a new User.");
            }

            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u =>
                    u.Name == userDTO.Name ||
                    u.PasswordHash == userDTO.PasswordHash);

            if (existingUser != null)
            {
                return BadRequest("Username or email already exists.");
            }

            var user = _mapper.Map<User>(userDTO);
            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            return CreatedAtRoute("GetUser",
                new { id = user.Id },
                user);
        }

        // PUT: api/User/{id}
        [HttpPut("{id:int}", Name = "UpdateUser")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUserAsync(
            int id,
            [FromBody] UserDTO userDTO)
        {
            if (userDTO == null || id != userDTO.Id)
            {
                return BadRequest("Invalid request data.");
            }

            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null)
            {
                return BadRequest("User not found.");
            }

            var duplicateUsername = await _context.Users
                .AnyAsync(u => u.Name == userDTO.Name && u.Id != id);

            if (duplicateUsername)
            {
                return BadRequest("Username or email already exists.");
            }

            var updatedUser = _mapper.Map<User>(userDTO);
            _context.Users.Update(updatedUser);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/User/{id}
        [HttpDelete("{id:int}", Name = "DeleteUser")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteUserAsync(int id)
        {
            if (id < 0)
            {
                return BadRequest("Invalid ID.");
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
