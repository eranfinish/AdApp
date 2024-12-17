using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AdApp.Core.Services;
using AdApp.Models;
using AdApp.Core.Services.Auth;
using AdApp.Core.Handlers.JWT;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;


namespace AdApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IValidator<User> _userValidator;
        private readonly IAuthService _authService;
        private readonly IJWT_Handler _jwtHandler;
        private readonly IConfiguration _config;
        public UserController(IValidator<User> userValidator, IConfiguration config, IAuthService authService, IJWT_Handler jwtHandler)
        {
            _userValidator = userValidator;
            _authService = authService;
             _jwtHandler = jwtHandler;
            _config = config;
        }

        [Authorize] 
        [HttpGet("status")]
        public IActionResult CheckLoginStatus()
        {
            var userId = User.Identity?.Name; 
            if (userId != null)
            {
                return Ok(new { message = "User is logged in", userId });
            }
            return Unauthorized();
        }

        [HttpPost("register")]
        public IActionResult Register(User user)
        {
            // Perform validation
            var validationResult = _userValidator.Validate(user);
            if (!validationResult.IsValid)
            {
                // Return validation errors
                return BadRequest(validationResult.Errors);
            }
            var token = _jwtHandler.GenerateJWT(user, _config); 
            
            Response.Cookies.Append("jwt", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                Expires = DateTime.UtcNow.AddDays(1),              
                SameSite = SameSiteMode.None, //For DEV
                Path = "/" //For DEV
            });
            // Call the service to register the user
            var result = _authService.RegisterAsync(user);

         
            return Ok(result.Result);
        }


        [HttpPost("login")]
        public IActionResult Login(User user)
        {
            // Validating User
            var validationResult = _userValidator.Validate(user);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors); 
            }

            
            var loginResult = _authService.Login(user.Email, user.Password);
            if (loginResult.Result == null)
            {
                return Unauthorized("Invalid credentials.");
            }
            var token = _jwtHandler.GenerateJWT(loginResult.Result, _config);

           
            Response.Cookies.Append("jwt", token, new CookieOptions
            {
                HttpOnly = false,
                Secure = false,
                SameSite = SameSiteMode.None, //For DEV
                Expires = DateTime.UtcNow.AddDays(1),
                Path = "/" //For DEV
            });

            return Ok( loginResult.Result);
        }
    }
}
