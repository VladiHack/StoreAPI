using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StoreAPI.DTO;
using StoreAPI.Models;
using StoreAPI.Services.CollectionProducts;

namespace StoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollectionProductController : ControllerBase
    {
        private readonly ICollectionProductService _collectionProductService;
        private readonly IMapper _mapper;

        public CollectionProductController(ICollectionProductService collectionProductService, IMapper mapper)
        {
            _collectionProductService = collectionProductService;
            _mapper = mapper;
        }

        // GET: api/CollectionProduct
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CollectionProduct>>> GetCollectionProductsAsync()
        {
            var collectionProducts = await _collectionProductService.GetCollectionProductsAsync();
            return Ok(collectionProducts);
        }

        // GET: api/CollectionProduct/{id}
        [HttpGet("{id:int}", Name = "GetCollectionProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CollectionProduct>> GetCollectionProductAsync(int id)
        {
            if (id < 0)
            {
                return BadRequest("Invalid ID.");
            }

            var collectionProduct = await _collectionProductService.GetCollectionProductByIdAsync(id);

            if (collectionProduct == null)
            {
                return NotFound("Collection Product not found.");
            }

            return Ok(collectionProduct);
        }

        // POST: api/CollectionProduct
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CollectionProduct>> CreateCollectionProductAsync(
            [FromBody] CollectionProductDTO collectionProductDTO)
        {
            if (collectionProductDTO == null)
            {
                return BadRequest("CollectionProductDTO cannot be null.");
            }

            if (collectionProductDTO.Id > 0)
            {
                return BadRequest("Id should not be provided for a new Collection Product.");
            }

            var existingCollectionProduct = await _collectionProductService.ExistsByIdAsync(collectionProductDTO.Id);

            if (existingCollectionProduct)
            {
                return BadRequest("Collection Product already exists.");
            }

            await _collectionProductService.CreateCollectionProductAsync(collectionProductDTO);

            var createdCollectionProduct = _mapper.Map<CollectionProduct>(collectionProductDTO);

            return CreatedAtRoute("GetCollectionProduct",
                new { id = createdCollectionProduct.Id },
                createdCollectionProduct);
        }

        // PUT: api/CollectionProduct/{id}
        [HttpPut("{id:int}", Name = "UpdateCollectionProduct")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateCollectionProductAsync(
            int id,
            [FromBody] CollectionProductDTO collectionProductDTO)
        {
            if (collectionProductDTO == null || id != collectionProductDTO.Id)
            {
                return BadRequest("Invalid request data.");
            }

            var existingCollectionProduct = await _collectionProductService.GetCollectionProductByIdAsync(id);

            if (existingCollectionProduct == null)
            {
                return BadRequest("Collection Product not found.");
            }

            await _collectionProductService.EditCollectionProductAsync(collectionProductDTO);

            return NoContent();
        }

        // DELETE: api/CollectionProduct/{id}
        [HttpDelete("{id:int}", Name = "DeleteCollectionProduct")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteCollectionProductAsync(int id)
        {
            if (id < 0)
            {
                return BadRequest("Invalid ID.");
            }

            var collectionProduct = await _collectionProductService.GetCollectionProductByIdAsync(id);

            if (collectionProduct == null)
            {
                return NotFound("Collection Product not found.");
            }

            await _collectionProductService.DeleteCollectionProductByIdAsync(id);

            return NoContent();
        }
    }
}
