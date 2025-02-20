using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreAPI.DTO;
using StoreAPI.Models;

namespace StoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreOptionController : ControllerBase
    {
        private readonly StoreDbContext _context;
        private readonly IMapper _mapper;

        public StoreOptionController(StoreDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/StoreOption
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<StoreOption>>> GetStoreOptionsAsync()
        {
            return Ok(await _context.StoreOptions.ToListAsync());
        }

        // GET: api/StoreOption/{id}
        [HttpGet("{id:int}", Name = "GetStoreOption")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<StoreOption>> GetStoreOptionAsync(int id)
        {
            if (id < 0)
            {
                return BadRequest("Invalid ID.");
            }

            var storeOption = await _context.StoreOptions
                .FirstOrDefaultAsync(so => so.Id == id);

            if (storeOption == null)
            {
                return NotFound("Store Option not found.");
            }

            return Ok(storeOption);
        }

        // POST: api/StoreOption
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<StoreOption>> CreateStoreOptionAsync(
            [FromBody] StoreOptionDTO storeOptionDTO)
        {
            if (storeOptionDTO == null)
            {
                return BadRequest("StoreOptionDTO cannot be null.");
            }

            if (storeOptionDTO.Id > 0)
            {
                return BadRequest("Id should not be provided for a new Store Option.");
            }

            var storeOption = _mapper.Map<StoreOption>(storeOptionDTO);
            _context.StoreOptions.Add(storeOption);

            await _context.SaveChangesAsync();
            return CreatedAtRoute("GetStoreOption",
                new { id = storeOption.Id },
                storeOption);
        }

        // PUT: api/StoreOption/{id}
        [HttpPut("{id:int}", Name = "UpdateStoreOption")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateStoreOptionAsync(
            int id,
            [FromBody] StoreOptionDTO storeOptionDTO)
        {
            if (storeOptionDTO == null || id != storeOptionDTO.Id)
            {
                return BadRequest("Invalid request data.");
            }

            var storeOption = _mapper.Map<StoreOption>(storeOptionDTO);
            _context.StoreOptions.Update(storeOption);

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/StoreOption/{id}
        [HttpDelete("{id:int}", Name = "DeleteStoreOption")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteStoreOptionAsync(int id)
        {
            if (id < 0)
            {
                return BadRequest("Invalid ID.");
            }

            var storeOption = await _context.StoreOptions
                .FirstOrDefaultAsync(so => so.Id == id);

            if (storeOption == null)
            {
                return NotFound("Store Option not found.");
            }

            _context.StoreOptions.Remove(storeOption);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
