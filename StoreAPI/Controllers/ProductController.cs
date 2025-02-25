using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreAPI.DTO;
using StoreAPI.Models;

namespace StoreAPI.Controllers
{
    [Route("api/Product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly StoreDbContext _context;
        private readonly IMapper _mapper;

        public ProductController(StoreDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Product
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsAsync()
        {
            return Ok(await _context.Products.ToListAsync());
        }

        // GET: api/Product/{id}
        [HttpGet("{id:int}", Name = "GetProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> GetProductAsync(int id)
        {
            if (id < 0)
            {
                return BadRequest("Invalid ID.");
            }

            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return NotFound("Product not found.");
            }

            return Ok(product);
        }

        // POST: api/Product
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Product>> CreateProductAsync([FromBody] ProductDTO productDTO)
        {
            if (productDTO == null)
            {
                return BadRequest("ProductDTO cannot be null.");
            }
            if (productDTO.Id > 0)
            {
                return BadRequest("Id should not be provided for a new Product.");
            }

            var product = _mapper.Map<Product>(productDTO);

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
        }

        // DELETE: api/Product/{id}
        [HttpDelete("{id:int}", Name = "DeleteProduct")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteProductAsync(int id)
        {
            if (id < 0)
            {
                return BadRequest("Invalid ID.");
            }

            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return NotFound("Product not found.");
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT: api/Product/{id}
        [HttpPut("{id:int}", Name = "UpdateProduct")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateProductAsync(int id, [FromBody] ProductDTO productDTO)
        {
            if (productDTO == null || id != productDTO.Id)
            {
                return BadRequest("Invalid request data.");
            }

            var product = _mapper.Map<Product>(productDTO);

            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        // GET: api/Product/paginated/{offset}/{limit}/{storeId}
        [HttpGet("paginated/{offset:int}/{limit:int}/{storeId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<object>>> GetPaginatedProductsAsync(int offset, int limit, int storeId)
        {
            var products = await _context.Products
                .Where(p => p.StoreId == storeId)  // Filter products by storeId
                .Skip(offset)
                .Take(limit)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Price,
                    Images = p.ProductImages.Where(img => img.IsTitleImage)
                                            .Select(img => new { img.ImageUrl, img.IsTitleImage })
                })
                .ToListAsync();

            return Ok(products);
        }


        // GET: api/Product/products_count/{storeId}
        [HttpGet("products_count/{storeId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<int>> GetProductsCountAsync(int storeId)
        {
            // Count only the products that belong to the specified store
            var productCount = await _context.Products
                                             .Where(p => p.StoreId == storeId)
                                             .CountAsync();

            return Ok(productCount);
        }


        // GET: api/Product/store/{storeId}/{slug}
        [HttpGet("store/{storeId:int}/{slug}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<object>> GetProductBySlugAndStoreAsync(int storeId, string slug)
        {
            var product = await _context.Products
                .Where(p => p.StoreId == storeId && p.Slug.ToLower() == slug.ToLower())  // Filter by storeId and slug
                .Include(p => p.ProductImages)
                .Include(p => p.ProductVariants)
                    .ThenInclude(pv => pv.Variant)
                .Include(p => p.ProductTypes)  // Include ProductType
                    .ThenInclude(pt => pt.Type) // Include VariantType
                .FirstOrDefaultAsync();

            if (product == null)
            {
                return NotFound("Product not found.");
            }

            // Select the data to be returned
            var result = new
            {
                product.Id,
                product.Slug,
                product.Name,
                product.Price,
                product.Quantity,
                product.Description,
                Images = product.ProductImages
                    .Where(img => img.ProductId == product.Id)
                    .Select(img => new
                    {
                        img.ImageUrl,
                        img.IsTitleImage
                    }).ToList(),
                Variants = product.ProductVariants
                    .Where(pv => pv.ProductId == product.Id)
                    .Select(pv => new
                    {
                        pv.VariantId,
                        VariantName = pv.Variant.Name
                    }).ToList(),
                VariantTypes = product.ProductTypes
                    .Where(pt => pt.ProductId == product.Id)
                    .Select(pt => new
                    {
                        pt.Type.Id,
                        VariantTypeName = pt.Type.Name,
                        pt.Type.Quantity
                    }).ToList()
            };

            return Ok(result);
        }



        // GET: api/Product/products_search/{searchedText}/{storeId}
        [HttpGet("products_search/{searchedText}/{storeId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<object>>> SearchProductsByNameAsync(string searchedText, int storeId)
        {
            // Perform a case-insensitive search using EF.Functions.Like and filter by storeId
            var products = await _context.Products
                .Where(p => p.StoreId == storeId && EF.Functions.Like(p.Name.ToLower(), $"%{searchedText.ToLower()}%"))  // Case-insensitive LIKE and store filter
                .Include(p => p.ProductImages)
                .Include(p => p.ProductVariants)
                    .ThenInclude(pv => pv.Variant)
                .Include(p => p.ProductTypes)  // Include ProductType (connection table)
                    .ThenInclude(pt => pt.Type) // Include VariantType
                .ToListAsync();

            if (!products.Any())
            {
                return NotFound("No products found.");
            }

            // Select and format the results
            var result = products.Select(product => new
            {
                product.Id,
                product.Slug,
                product.Name,
                product.Price,
                product.Quantity,
                product.Description,
                Images = product.ProductImages
                    .Where(img => img.ProductId == product.Id)
                    .Select(img => new
                    {
                        img.ImageUrl,
                        img.IsTitleImage
                    }).ToList(),
                Variants = product.ProductVariants
                    .Where(pv => pv.ProductId == product.Id)
                    .Select(pv => new
                    {
                        pv.VariantId,
                        VariantName = pv.Variant.Name,
                        VariantQuantity = pv.Variant.Quantity // 🔥 Includes Variant Quantity
                    }).ToList(),
                VariantTypes = product.ProductTypes
                    .Where(pt => pt.ProductId == product.Id)
                    .Select(pt => new
                    {
                        pt.Type.Id,
                        VariantTypeName = pt.Type.Name,
                        VariantTypeQuantity = pt.Type.Quantity // 🔥 Includes VariantType Quantity
                    }).ToList()
            }).ToList();

            return Ok(result);
        }







    }
}
