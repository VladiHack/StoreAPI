using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreAPI.DTO;
using StoreAPI.Models;

namespace StoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VariantTypeController : ControllerBase
    {
        private readonly StoreDbContext _context;
        private readonly IMapper _mapper;

        public VariantTypeController(StoreDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/VariantType
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<VariantType>>> GetVariantTypesAsync()
        {
            return Ok(await _context.VariantTypes.ToListAsync());
        }

        // GET: api/VariantType/{id}
        [HttpGet("{id:int}", Name = "GetVariantType")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VariantType>> GetVariantTypeAsync(int id)
        {
            if (id < 0)
            {
                return BadRequest("Invalid ID.");
            }

            var variantType = await _context.VariantTypes
                .FirstOrDefaultAsync(v => v.Id == id);

            if (variantType == null)
            {
                return NotFound("Variant Type not found.");
            }

            return Ok(variantType);
        }

        // POST: api/VariantType
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<VariantType>> CreateVariantTypeAsync(
            [FromBody] VariantTypeDTO variantTypeDTO)
        {
            if (variantTypeDTO == null)
            {
                return BadRequest("VariantTypeDTO cannot be null.");
            }

            if (variantTypeDTO.Id > 0)
            {
                return BadRequest("Id should not be provided for a new Variant Type.");
            }

            var existingVariantType = await _context.VariantTypes
                .FirstOrDefaultAsync(v => v.Name == variantTypeDTO.Name);

            if (existingVariantType != null)
            {
                return BadRequest("Variant Type already exists.");
            }

            var variantType = _mapper.Map<VariantType>(variantTypeDTO);
            _context.VariantTypes.Add(variantType);

            await _context.SaveChangesAsync();

            return CreatedAtRoute("GetVariantType",
                new { id = variantType.Id },
                variantType);
        }

        // PUT: api/VariantType/{id}
        [HttpPut("{id:int}", Name = "UpdateVariantType")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateVariantTypeAsync(
            int id,
            [FromBody] VariantTypeDTO variantTypeDTO)
        {
            if (variantTypeDTO == null || id != variantTypeDTO.Id)
            {
                return BadRequest("Invalid request data.");
            }

            var existingVariantType = await _context.VariantTypes.FindAsync(id);

            if (existingVariantType == null)
            {
                return BadRequest("Variant Type not found.");
            }

            var duplicateVariantType = await _context.VariantTypes
                .AnyAsync(v => v.Name==variantTypeDTO.Name && v.Id != id);

            if (duplicateVariantType)
            {
                return BadRequest("Variant Type already exists.");
            }

            var updatedVariantType = _mapper.Map<VariantType>(variantTypeDTO);
            _context.VariantTypes.Update(updatedVariantType);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/VariantType/{id}
        [HttpDelete("{id:int}", Name = "DeleteVariantType")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteVariantTypeAsync(int id)
        {
            if (id < 0)
            {
                return BadRequest("Invalid ID.");
            }

            var variantType = await _context.VariantTypes
                .FirstOrDefaultAsync(v => v.Id == id);

            if (variantType == null)
            {
                return NotFound("Variant Type not found.");
            }

            _context.VariantTypes.Remove(variantType);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
