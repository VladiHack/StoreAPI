using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreAPI.DTO;
using StoreAPI.Models;

namespace StoreAPI.Controllers
{
    [Route("api/BannerSlide")]
    [ApiController]
    public class BannerSlideController : ControllerBase
    {
        private readonly StoreDbContext _context;
        private readonly IMapper _mapper;

        public BannerSlideController(StoreDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("store/{storeId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<BannerSlide>>> GetBannerSlidesAsync(int storeId)
        {
            // Filter BannerSlides where the associated Banner's StoreId matches the provided storeId
            var bannerSlides = await _context.BannerSlides
                .Include(bs => bs.Banner) // Include the Banner entity to access its StoreId
                .Where(bs => bs.Banner != null && bs.Banner.StoreId == storeId)
                .ToListAsync();

            return Ok(bannerSlides);
        }

        [HttpGet("{id:int}", Name = "GetBannerSlide")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BannerSlide>> GetBannerSlideAsync(int id)
        {
            if (id < 0)
            {
                return BadRequest();
            }
            var bannerSlide = await _context.BannerSlides.FirstOrDefaultAsync(u => u.Id == id);
            if (bannerSlide == null)
            {
                return NotFound();
            }
            return Ok(bannerSlide);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BannerSlide>> CreateBannerSlideAsync([FromBody] BannerSlideDTO bannerSlideDTO)
        {
            if (bannerSlideDTO == null)
            {
                return BadRequest(bannerSlideDTO);
            }
            if (bannerSlideDTO.Id > 0)
            {
                return BadRequest("Id should not be provided for a new BannerSlide.");
            }

            var bannerSlide = _mapper.Map<BannerSlide>(bannerSlideDTO);

            _context.BannerSlides.Add(bannerSlide);
            await _context.SaveChangesAsync();
            return CreatedAtRoute("GetBannerSlide", new { id = bannerSlideDTO.Id }, bannerSlideDTO);
        }

        [HttpDelete("{id:int}", Name = "DeleteBannerSlide")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteBannerSlideAsync(int id)
        {
            if (id < 0)
            {
                return BadRequest();
            }
            var bannerSlide = await _context.BannerSlides.FirstOrDefaultAsync(u => u.Id == id);
            if (bannerSlide == null)
            {
                return NotFound();
            }

            _context.BannerSlides.Remove(bannerSlide);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id:int}", Name = "UpdateBannerSlide")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateBannerSlideAsync(int id, [FromBody] BannerSlideDTO bannerSlideDTO)
        {
            if (bannerSlideDTO == null || id != bannerSlideDTO.Id)
            {
                return BadRequest();
            }

            var bannerSlide = _mapper.Map<BannerSlide>(bannerSlideDTO);

            _context.BannerSlides.Update(bannerSlide);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
