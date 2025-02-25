using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StoreAPI.DTO;
using StoreAPI.Models;
using StoreAPI.Services.ProductTypes;

namespace StoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductTypeController : ControllerBase
    {
        private readonly IProductTypeService _productTypeService;
        private readonly IMapper _mapper;

        public ProductTypeController(IProductTypeService productTypeService, IMapper mapper)
        {
            _productTypeService = productTypeService;
            _mapper = mapper;
        }

        // GET: api/ProductType
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ProductType>>> GetProductTypesAsync()
        {
            var productTypes = await _productTypeService.GetProductTypesAsync();
            return Ok(productTypes);
        }

        // GET: api/ProductType/{id}
        [HttpGet("{id:int}", Name = "GetProductType")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductType>> GetProductTypeAsync(int id)
        {
            if (id < 0)
            {
                return BadRequest("Invalid ID.");
            }

            var productType = await _productTypeService.GetProductTypeByIdAsync(id);

            if (productType == null)
            {
                return NotFound("Product Type not found.");
            }

            return Ok(productType);
        }

        // POST: api/ProductType
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProductType>> CreateProductTypeAsync(
            [FromBody] ProductTypeDTO productTypeDTO)
        {
            if (productTypeDTO == null)
            {
                return BadRequest("ProductTypeDTO cannot be null.");
            }

            if (productTypeDTO.Id > 0)
            {
                return BadRequest("Id should not be provided for a new Product Type.");
            }

            var existingProductType = await _productTypeService.ExistsByIdAsync(productTypeDTO.Id);

            if (existingProductType)
            {
                return BadRequest("Product Type already exists.");
            }

            await _productTypeService.CreateProductTypeAsync(productTypeDTO);

            var createdProductType = _mapper.Map<ProductType>(productTypeDTO);

            return CreatedAtRoute("GetProductType",
                new { id = createdProductType.Id },
                createdProductType);
        }

        // PUT: api/ProductType/{id}
        [HttpPut("{id:int}", Name = "UpdateProductType")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateProductTypeAsync(
            int id,
            [FromBody] ProductTypeDTO productTypeDTO)
        {
            if (productTypeDTO == null || id != productTypeDTO.Id)
            {
                return BadRequest("Invalid request data.");
            }

            var existingProductType = await _productTypeService.GetProductTypeByIdAsync(id);

            if (existingProductType == null)
            {
                return BadRequest("Product Type not found.");
            }

            await _productTypeService.EditProductTypeAsync(productTypeDTO);

            return NoContent();
        }

        // DELETE: api/ProductType/{id}
        [HttpDelete("{id:int}", Name = "DeleteProductType")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteProductTypeAsync(int id)
        {
            if (id < 0)
            {
                return BadRequest("Invalid ID.");
            }

            var productType = await _productTypeService.GetProductTypeByIdAsync(id);

            if (productType == null)
            {
                return NotFound("Product Type not found.");
            }

            await _productTypeService.DeleteProductTypeByIdAsync(id);

            return NoContent();
        }
    }
}
