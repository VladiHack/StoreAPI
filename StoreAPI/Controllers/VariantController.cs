using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreAPI.DTO;
using StoreAPI.Models;

namespace StoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VariantController : ControllerBase
    {
        private readonly StoreDbContext _context;
        private readonly IMapper _mapper;

        public VariantController(StoreDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Variant
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Variant>>> GetVariantsAsync()
        {
            return Ok(await _context.Variants.ToListAsync());
        }

        // GET: api/Variant/{id}
        [HttpGet("{id:int}", Name = "GetVariant")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Variant>> GetVariantAsync(int id)
        {
            if (id < 0)
            {
                return BadRequest("Invalid ID.");
            }

            var variant = await _context.Variants
                .FirstOrDefaultAsync(v => v.Id == id);

            if (variant == null)
            {
                return NotFound("Variant not found.");
            }

            return Ok(variant);
        }

        // POST: api/Variant
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Variant>> CreateVariantAsync(
            [FromBody] VariantDTO variantDTO)
        {
            if (variantDTO == null)
            {
                return BadRequest("VariantDTO cannot be null.");
            }

            if (variantDTO.Id > 0)
            {
                return BadRequest("Id should not be provided for a new Variant.");
            }

            // Add specific validation for variants here if needed
            var existingVariant = await _context.Variants
                .FirstOrDefaultAsync(v => v.Name ==variantDTO.Name/* add your duplicate check logic */);

            if (existingVariant != null)
            {
                return BadRequest("Variant already exists.");
            }

            var variant = _mapper.Map<Variant>(variantDTO);
            _context.Variants.Add(variant);

            await _context.SaveChangesAsync();

            return CreatedAtRoute("GetVariant",
                new { id = variant.Id },
                variant);
        }

        // PUT: api/Variant/{id}
        [HttpPut("{id:int}", Name = "UpdateVariant")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateVariantAsync(
            int id,
            [FromBody] VariantDTO variantDTO)
        {
            if (variantDTO == null || id != variantDTO.Id)
            {
                return BadRequest("Invalid request data.");
            }

            var existingVariant = await _context.Variants.FindAsync(id);
            if (existingVariant == null)
            {
                return BadRequest("Variant not found.");
            }

            // Add specific validation for variants here if needed
            var duplicateVariant = await _context.Variants
                .AnyAsync(v => v.Name==variantDTO.Name && v.Id != id);

            if (duplicateVariant)
            {
                return BadRequest("Variant already exists.");
            }

            var updatedVariant = _mapper.Map<Variant>(variantDTO);
            _context.Variants.Update(updatedVariant);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Variant/{id}
        [HttpDelete("{id:int}", Name = "DeleteVariant")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteVariantAsync(int id)
        {
            if (id < 0)
            {
                return BadRequest("Invalid ID.");
            }

            var variant = await _context.Variants
                .FirstOrDefaultAsync(v => v.Id == id);

            if (variant == null)
            {
                return NotFound("Variant not found.");
            }

            _context.Variants.Remove(variant);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
