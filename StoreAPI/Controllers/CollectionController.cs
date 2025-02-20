using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreAPI.DTO;
using StoreAPI.Models;

namespace StoreAPI.Controllers
{
    [Route("api/Collection")]
    [ApiController]
    public class CollectionController : ControllerBase
    {
        private readonly StoreDbContext _context;
        private readonly IMapper _mapper;

        public CollectionController(StoreDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Collection
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<object>>> GetCollectionsAsync(int storeId)
        {
            var collections = await _context.Collections
                .Where(c => c.StoreId == storeId) // Filter collections by storeId
                .Select(c => new
                {
                    c.Id,
                    c.Title,
                    c.ImageUrl,
                    Products = c.CollectionProducts
                        .Where(cp => cp.Product.StoreId == storeId) // Filter products by storeId
                        .Select(cp => new
                        {
                            ProductId = cp.Product.Id, // Custom property name, no $id
                            cp.Product.Name,
                            cp.Product.Price,
                            cp.Product.Quantity,
                            cp.Product.Description
                        })
                        .ToList() // Convert to list to avoid potential issues with deferred execution
                })
                .ToListAsync();

            return Ok(collections);
        }

        // GET: api/Collection/WithoutProducts/store/{storeId}
        [HttpGet("WithoutProducts/store/{storeId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<object>>> GetCollectionsWithoutProductsByStoreAsync(int storeId)
        {
            var collections = await _context.Collections
                .Where(c => c.StoreId == storeId) // Filter collections by storeId
                .Select(c => new
                {
                    c.Id,
                    c.Title,
                    c.ImageUrl
                })
                .ToListAsync();

            return Ok(collections);
        }


        // GET: api/Collection/{id}
        [HttpGet("{id:int}", Name = "GetCollection")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetCollectionAsync(int id)
        {
            if (id < 0)
            {
                return BadRequest("Invalid ID.");
            }

            var collection = await _context.Collections
                .Where(c => c.Id == id)
                .Select(c => new
                {
                    c.Id,
                    c.Title,
                    c.ImageUrl,
                    Products = c.CollectionProducts
                        .Select(cp => new
                        {
                            cp.Product.Id,
                            cp.Product.Name,
                            cp.Product.Price,
                            cp.Product.Quantity,
                            cp.Product.Description
                        })
                })
                .FirstOrDefaultAsync();

            if (collection == null)
            {
                return NotFound("Collection not found.");
            }

            // Return the collection with default JSON settings (no $id fields)
            return Ok(collection);
        }

        // POST: api/Collection
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Collection>> CreateCollectionAsync([FromBody] CollectionDTO collectionDTO)
        {
            if (collectionDTO == null)
            {
                return BadRequest("CollectionDTO cannot be null.");
            }
            if (collectionDTO.Id > 0)
            {
                return BadRequest("Id should not be provided for a new Collection.");
            }

            var collection = _mapper.Map<Collection>(collectionDTO);

            _context.Collections.Add(collection);
            await _context.SaveChangesAsync();

            return CreatedAtRoute("GetCollection", new { id = collection.Id }, collection);
        }

        // DELETE: api/Collection/{id}
        [HttpDelete("{id:int}", Name = "DeleteCollection")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteCollectionAsync(int id)
        {
            if (id < 0)
            {
                return BadRequest("Invalid ID.");
            }

            var collection = await _context.Collections.FirstOrDefaultAsync(u => u.Id == id);
            if (collection == null)
            {
                return NotFound("Collection not found.");
            }

            _context.Collections.Remove(collection);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT: api/Collection/{id}
        [HttpPut("{id:int}", Name = "UpdateCollection")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateCollectionAsync(int id, [FromBody] CollectionDTO collectionDTO)
        {
            if (collectionDTO == null || id != collectionDTO.Id)
            {
                return BadRequest("Invalid request data.");
            }

            var collection = _mapper.Map<Collection>(collectionDTO);

            _context.Collections.Update(collection);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}