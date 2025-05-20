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
    
    [Authorize]
    [HttpPut("update-password")]
    public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordDto dto)
    {
        var userId = HttpContext.GetUserId();
        if (userId == null)
            return Unauthorized(new { message = "User not authenticated." });

        var result = await userService.UpdatePasswordAsync(userId.Value, dto.CurrentPassword, dto.NewPassword);

        return result.Success
            ? Ok(new { message = result.Message })
            : BadRequest(new { message = result.Message });
    }
    
    [Authorize]
    [HttpDelete("delete-account")]
    public async Task<IActionResult> DeleteAccount([FromBody] string password)
    {
        var userId = HttpContext.GetUserId(); // gets user ID from token

        var result = await userService.DeleteAccountAsync(userId.Value, password);
        if (!result.Success)
            return BadRequest(new { message = result.Message });

        return Ok(new { message = result.Message });
    }

}