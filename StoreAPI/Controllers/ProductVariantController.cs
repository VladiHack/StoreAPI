using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreAPI.DTO;
using StoreAPI.Models;

namespace StoreAPI.Controllers
{
    [Route("api/ProductVariant")]
    [ApiController]
    public class ProductVariantController : ControllerBase
    {
        private readonly StoreDbContext _context;
        private readonly IMapper _mapper;

        public ProductVariantController(StoreDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/ProductVariant
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ProductVariant>>> GetProductVariantsAsync()
        {
            return Ok(await _context.ProductVariants.ToListAsync());
        }

        // GET: api/ProductVariant/{id}
        [HttpGet("{id:int}", Name = "GetProductVariant")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductVariant>> GetProductVariantAsync(int id)
        {
            if (id < 0)
            {
                return BadRequest("Invalid ID.");
            }

            var productVariant = await _context.ProductVariants
                .FirstOrDefaultAsync(pv => pv.Id == id);

            if (productVariant == null)
            {
                return NotFound("Product Variant not found.");
            }

            return Ok(productVariant);
        }

        // POST: api/ProductVariant
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProductVariant>> CreateProductVariantAsync(
            [FromBody] ProductVariantDTO productVariantDTO)
        {
            if (productVariantDTO == null)
            {
                return BadRequest("ProductVariantDTO cannot be null.");
            }

            if (productVariantDTO.Id > 0)
            {
                return BadRequest("Id should not be provided for a new Product Variant.");
            }

            var productVariant = _mapper.Map<ProductVariant>(productVariantDTO);
            _context.ProductVariants.Add(productVariant);

            await _context.SaveChangesAsync();

            return CreatedAtRoute("GetProductVariant",
                new { id = productVariant.Id },
                productVariant);
        }

        // PUT: api/ProductVariant/{id}
        [HttpPut("{id:int}", Name = "UpdateProductVariant")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateProductVariantAsync(
            int id,
            [FromBody] ProductVariantDTO productVariantDTO)
        {
            if (productVariantDTO == null || id != productVariantDTO.Id)
            {
                return BadRequest("Invalid request data.");
            }

            var productVariant = _mapper.Map<ProductVariant>(productVariantDTO);
            _context.ProductVariants.Update(productVariant);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/ProductVariant/{id}
        [HttpDelete("{id:int}", Name = "DeleteProductVariant")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteProductVariantAsync(int id)
        {
            if (id < 0)
            {
                return BadRequest("Invalid ID.");
            }

            var productVariant = await _context.ProductVariants
                .FirstOrDefaultAsync(pv => pv.Id == id);

            if (productVariant == null)
            {
                return NotFound("Product Variant not found.");
            }

            _context.ProductVariants.Remove(productVariant);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
