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

        if (!result.Success)
            return Unauthorized(result);

        return Ok(result);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromForm] RegisterDto registerDto)
    {
        var result = await userService.RegisterUser(registerDto);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }
}