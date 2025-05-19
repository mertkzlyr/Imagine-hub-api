using ImagineHubAPI.DTOs.UserDTOs;
using ImagineHubAPI.Extensions;
using ImagineHubAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImagineHubAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetUserById()
    {
        try
        {
            var userId = HttpContext.GetUserId();
            if (userId == null)
                return Unauthorized(new { Message = "User not authenticated." });
            
            var user = await userService.GetUserByIdAsync(userId.Value);
            return Ok(user);
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { Message = "User not found." });
        }
    }
    
    [HttpGet("by-username/{username}")]
    public async Task<IActionResult> GetUserByUsername(string username)
    {
        var user = await userService.GetByUsernameAsync(username);
        if (user == null)
        {
            return NotFound(user);
        }

        return Ok(user);
    }
    
    [Authorize] // Optional: only allow authenticated users
    [HttpPost("update-profile-picture")]
    public async Task<IActionResult> UpdateProfilePicture([FromForm] UpdatePpDto dto)
    {
        if (dto.ProfilePicture == null || dto.ProfilePicture.Length == 0)
            return BadRequest(new { Message = "No file uploaded." });

        var userId = HttpContext.GetUserId();
        if (userId == null)
            return Unauthorized(new { Message = "User not authenticated." });

        var result = await userService.UpdateProfilePictureAsync(userId.Value, dto.ProfilePicture);
        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }
    
    [HttpPut("update")]
    [Authorize(Policy = "UserPolicy")]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto updateDto)
    {
        var userId = HttpContext.GetUserId();
        if (userId == null)
            return Unauthorized(new { Message = "User not authenticated." });

        var result = await userService.UpdateUserAsync(userId.Value, updateDto);

        if (!result.Success)
            return BadRequest(new { Message = result.Message });

        return Ok(result);
    }
}