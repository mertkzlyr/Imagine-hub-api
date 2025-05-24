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
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetImageById(Guid id)
    {
        var userId = HttpContext.GetUserId();
        if (userId == null)
            return Unauthorized(new { message = "User not authenticated." });
        
        var result = await imageService.GetImageByIdAsync(id, userId.Value);

        if (!result.Success)
            return NotFound(new { message = result.Message });

        return Ok(result.Data);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetImagesByUserId([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var userId = HttpContext.GetUserId();
        if (userId == null)
            return Unauthorized(new { message = "User not authenticated." });

        var result = await imageService.GetImagesByUserIdAsync(userId.Value, page, pageSize);

        if (!result.Success)
            return NotFound(new { message = result.Message });

        return Ok(result);
    }
    
    [HttpGet("generation-tokens")]
    [Authorize]
    public async Task<IActionResult> GetGenerationTokens()
    {
        var userId = HttpContext.GetUserId();
        if (userId == null)
            return Unauthorized(new { message = "User not authenticated." });

        var user = await imageService.GetUserByIdAsync(userId.Value); // You’ll add this method next.

        if (user == null)
            return NotFound(new { message = "User not found." });

        return Ok(new { generationTokens = user.GenerationToken });
    }
    
    [HttpGet("stream/{filename}")]
    [Authorize]
    public IActionResult StreamImage(string filename)
    {
        var userId = HttpContext.GetUserId();
        if (userId == null)
            return Unauthorized(new { message = "User not authenticated." });

        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ai_pics", filename);
    
        if (!System.IO.File.Exists(filePath))
            return NotFound(new { message = "Image not found." });

        var contentType = "image/webp"; // or use a file extension check

        var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        return File(stream, contentType);
    }
}