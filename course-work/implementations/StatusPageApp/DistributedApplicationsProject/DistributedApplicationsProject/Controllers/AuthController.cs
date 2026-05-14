
using API.Services;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using StatusPageServices.Services;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserService userService;
    private readonly TokenServices tokenService;

    public AuthController(UserService userServiceInjection, TokenServices tokenServiceInjection)
    {
        userService = userServiceInjection;
        tokenService = tokenServiceInjection;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var user = userService.FindByUsername(request.Username);

        if (user == null || !userService.VerifyPassword(request.Password, user.Password))
            return Unauthorized("Invalid username or password");

        string token = tokenService.CreateToken(user);

        return Ok(token);
    }