using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AdApp.Models;
using AdApp.Core.Services.Ads;
using Microsoft.AspNetCore.Authorization;
namespace AdApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdsController : ControllerBase
    {
        private readonly AdService _adService;

        public AdsController()
        {
            _adService = new AdService();
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAllAds()
        {
            var ads = _adService.GetAllAds();
            return Ok(ads);
        }

        [Authorize]
        [HttpGet("{id}")]
        public IActionResult GetAdById(int id)
        {
            var ad = _adService.GetAdById(id);
            if (ad == null) return NotFound();
            return Ok(ad);
        }

        [Authorize]
        [HttpPost]
        public IActionResult CreateAd([FromBody] Ad ad)
        {
            if (ad.ownerId == 0) return BadRequest("OwnerId is required.");
            _adService.CreateAd(ad);
            return CreatedAtAction(nameof(GetAdById), new { id = ad.id }, ad);
        }

        [Authorize]
        [HttpPut]
        public IActionResult UpdateAd([FromBody] Ad ad)
        {
          
        //    string useename= context..Items["userName"].ToString();
            if (!_adService.UpdateAd( ad))
                return Unauthorized();
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult DeleteAd(int id)
        {
            if (!_adService.DeleteAd(id))
                return Unauthorized();
            return NoContent();
        }
    }

}
