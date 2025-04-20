using ImagineHubAPI.DTOs.AuthDTOs;
using ImagineHubAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ImagineHubAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IUserService userService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await userService.AuthenticateAsync(request);

        if (result == null)
            return Unauthorized(new { message = "Invalid email or password." });

        return Ok(result);
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        var result = await userService.RegisterUser(registerDto);

        if (result != "User registered successfully.")
            return BadRequest(new { message = result });

        return Ok(new { message = result });
    }
}