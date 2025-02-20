using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreAPI.DTO;
using StoreAPI.Models;

namespace StoreAPI.Controllers
{
    [Route("api/ProductImage")]
    [ApiController]
    public class ProductImageController : ControllerBase
    {
        private readonly StoreDbContext _context;
        private readonly IMapper _mapper;

        public ProductImageController(StoreDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/ProductImage
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ProductImage>>> GetProductImagesAsync()
        {
            return Ok(await _context.ProductImages.ToListAsync());
        }

        // GET: api/ProductImage/{id}
        [HttpGet("{id:int}", Name = "GetProductImage")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductImage>> GetProductImageAsync(int id)
        {
            if (id < 0)
            {
                return BadRequest("Invalid ID.");
            }

            var productImage = await _context.ProductImages
                .FirstOrDefaultAsync(pi => pi.Id == id);

            if (productImage == null)
            {
                return NotFound("Product Image not found.");
            }

            return Ok(productImage);
        }

        // POST: api/ProductImage
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProductImage>> CreateProductImageAsync(
            [FromBody] ProductImageDTO productImageDTO)
        {
            if (productImageDTO == null)
            {
                return BadRequest("ProductImageDTO cannot be null.");
            }
            if (productImageDTO.Id > 0)
            {
                return BadRequest("Id should not be provided for a new Product Image.");
            }

            var productImage = _mapper.Map<ProductImage>(productImageDTO);

            _context.ProductImages.Add(productImage);
            await _context.SaveChangesAsync();

            return CreatedAtRoute("GetProductImage",
                new { id = productImage.Id },
                productImage);
        }

        // DELETE: api/ProductImage/{id}
        [HttpDelete("{id:int}", Name = "DeleteProductImage")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteProductImageAsync(int id)
        {
            if (id < 0)
            {
                return BadRequest("Invalid ID.");
            }

            var productImage = await _context.ProductImages
                .FirstOrDefaultAsync(pi => pi.Id == id);

            if (productImage == null)
            {
                return NotFound("Product Image not found.");
            }

            _context.ProductImages.Remove(productImage);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT: api/ProductImage/{id}
        [HttpPut("{id:int}", Name = "UpdateProductImage")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateProductImageAsync(
            int id,
            [FromBody] ProductImageDTO productImageDTO)
        {
            if (productImageDTO == null || id != productImageDTO.Id)
            {
                return BadRequest("Invalid request data.");
            }

            var productImage = _mapper.Map<ProductImage>(productImageDTO);

            _context.ProductImages.Update(productImage);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
