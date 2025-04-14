using ImagineHubAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ImagineHubAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        try
        {
            var user = await userService.GetUserByIdAsync(id);
            return Ok(user);
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { Message = "User not found." });
        }
    }
}