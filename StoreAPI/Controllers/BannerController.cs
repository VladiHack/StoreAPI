using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreAPI.DTO;
using StoreAPI.Models;

namespace StoreAPI.Controllers
{
    [Route("api/Banner")]
    [ApiController]
    public class BannerController:ControllerBase
    {
        private readonly StoreDbContext _context;
        private readonly IMapper _mapper;

        public BannerController(StoreDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Banner/store/{storeId}
        [HttpGet("store/{storeId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Banner>>> GetBannersByStoreAsync(int storeId)
        {
            return Ok(await _context.Banners
                .Where(b => b.StoreId == storeId) // Filter banners by storeId
                .ToListAsync());
        }



        [HttpGet("{id:int}", Name = "GetBanner")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<object>> GetBannerAsync(int id)
        {
            if (id < 0)
            {
                return BadRequest();
            }

            var banner = await _context.Banners
                .Where(b => b.Id == id)
                .Select(b => new
                {
                    b.Id,
                    b.CreatedAt,
                    b.UpdatedAt,
                    BannerSlides = b.BannerSlides
                        .Select(bs => new
                        {
                            bs.Id,
                            bs.Title,
                            bs.Description,
                            bs.ImageUrl,
                            bs.Href,
                            bs.CreatedAt,
                            bs.UpdatedAt
                        })
                        .ToList() // Ensuring the BannerSlides are materialized into a list
                })
                .FirstOrDefaultAsync();

            if (banner == null)
            {
                return NotFound();
            }

            return Ok(banner);
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Banner>> CreateBannerAsync([FromBody] BannerDTO bannerDTO)
        {
            if (bannerDTO == null)
            {
                return BadRequest(bannerDTO);
            }
            if (bannerDTO.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            var banner = _mapper.Map<Banner>(bannerDTO);

            _context.Banners.Add(banner);
            await _context.SaveChangesAsync();
            return CreatedAtRoute("GetBanner", new { id = bannerDTO.Id }, bannerDTO);
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id:int}", Name = "DeleteBanner")]
        public async Task<ActionResult> DeleteBannerAsync(int id)
        {
            if (id < 0)
            {
                return BadRequest();
            }
            var banner = await _context.Banners.FirstOrDefaultAsync(u => u.Id == id);
            if (banner == null)
            {
                return NotFound();
            }

            // Delete all banners of the banner
            List<BannerSlide> bannerSlides = await _context.BannerSlides.Where(u => u.BannerId == id).ToListAsync();
            foreach (BannerSlide slide in bannerSlides)
            {
                _context.BannerSlides.Remove(slide);
                await _context.SaveChangesAsync();
            }

            _context.Banners.Remove(banner);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id:int}", Name = "UpdateBanner")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateBannerAsync(int id, [FromBody] BannerDTO bannerDTO)
        {
            if (bannerDTO == null || id != bannerDTO.Id)
            {
                return BadRequest();
            }

            var banner = _mapper.Map<Banner>(bannerDTO);

            _context.Banners.Update(banner);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
