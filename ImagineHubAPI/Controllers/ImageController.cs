using ImagineHubAPI.DTOs.ImageDTOs;
using ImagineHubAPI.Extensions;
using ImagineHubAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImagineHubAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImageController(IImageService imageService) : ControllerBase
{
    [HttpPost("generate-image")]
    [Authorize]
    public async Task<IActionResult> GenerateImage([FromBody] GenerateImageDto dto)
    {
        var userId = HttpContext.GetUserId();
        if (userId == null)
            return Unauthorized(new { message = "User not authenticated." });
        
        var result = await imageService.GenerateAndSaveImageAsync(dto.Prompt, userId.Value);

        if (!result.Success)
            return BadRequest(new { message = result.Message });

        return Ok(new { imageUrl = result.Data });
    }
}