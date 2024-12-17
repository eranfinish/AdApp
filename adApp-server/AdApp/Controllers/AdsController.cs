using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AdApp.Models;
using AdApp.Core.Services.Ads;
using Microsoft.AspNetCore.Authorization;
using AdApp.Core.Helpers.User;
namespace AdApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdsController : ControllerBase
    {
        private readonly IAdService _adService;
  private readonly IUserHelper _userHelper;
        public AdsController(IAdService adService, IUserHelper userHelper)
        {
            _adService =  adService;
            _userHelper = userHelper;
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAllAds()
        {
            var usename = Response.HttpContext.Items["userName"];
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
            var userName = Response.HttpContext.Items["userName"];
            User? currentUser = _userHelper.GetAllUsers()
                .FirstOrDefault(u => u.UserName == userName);
            
            if (currentUser == null) 
                return NotFound("User Not Found!");
            var ads = _adService.GetAllAds();
            var maxAdId = ads.Any() ? ads.Max(a => a.id) : 0;
            ad.id = maxAdId + 1;//New Id For Ad

            if (ad.ownerId == currentUser.Id) return BadRequest("OwnerId is required.");
            _adService.CreateAd(ad);
            return CreatedAtAction(nameof(GetAdById), new { id = ad.id }, ad);
        }

        [Authorize]
        [HttpPut]
        public  IActionResult UpdateAd([FromBody] Ad ad)
        {
            var userName = Response.HttpContext.Items["userName"];
            User? currentUser = _userHelper.GetAllUsers()
                .FirstOrDefault(u => u.UserName == userName);

            if (currentUser == null)
                return NotFound("User Not Found!");

            //    string useename= context..Items["userName"].ToString();
            if (! _adService.UpdateAd( ad))
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
