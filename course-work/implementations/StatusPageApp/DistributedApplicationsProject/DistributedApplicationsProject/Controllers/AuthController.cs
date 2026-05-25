using Microsoft.AspNetCore.Mvc;
using StatusPageServices.Interfaces;
using StatusPageServices.RequestDTO.Users.StatusPageServices.RequestDTO.Users;
using StatusPageServices.Services;

namespace StatusPageAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly TokenService _tokenService;
        private readonly IUserService _userService;

        public AuthController(TokenService tokenService, IUserService userService)
        {
            _tokenService = tokenService;
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            // Try to find user in Users table (seeded admin user expected)
            var user = await _userService.FindByUsernameAsync(request.Username);
            if (user is null) return Unauthorized("Invalid username or password");

            // Verify password (Note: currently plain-text comparison)
            if (!_userService.VerifyPassword(request.Password, user.Password))
                return Unauthorized("Invalid username or password");

            var token = _tokenService.CreateSimpleToken(request.Username);
            return Ok(new { Token = token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (await _userService.UsernameExistsAsync(request.Username))
            {
                return BadRequest("Username already taken");
            }

            var created = await _userService.CreateAsync(new CreateUserDto(request.Username, request.Password));
            return Ok(created);
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class RegisterRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}