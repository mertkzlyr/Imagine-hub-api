using ImagineHubAPI.DTOs.PostDTOs;
using ImagineHubAPI.Extensions;
using ImagineHubAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImagineHubAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostController(IPostService postService) : ControllerBase
{
    [HttpGet("posts")]
    public async Task<IActionResult> GetAllPosts([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await postService.GetAllPostsAsync(page, pageSize);

        if (!result.Success)
            return BadRequest(new { message = result.Message });

        return Ok(result);
    }
    
    [Authorize]
    [HttpPost("posts")]
    public async Task<IActionResult> CreatePost([FromBody] CreatePostDto createPostDto)
    {
        var userId = HttpContext.GetUserId();  // Custom helper to get the userId from the JWT token
        if (userId == null)
        {
            return Unauthorized(new { message = "User not authenticated." });
        }

        var result = await postService.CreatePostAsync(userId.Value, createPostDto);

        if (!result.Success)
        {
            return BadRequest(new { message = result.Message });
        }

        return Ok(result);  // Post created successfully, return result with PostDto
    }
    
    [HttpGet("posts/{id}")]
    [Authorize]
    public async Task<IActionResult> GetPostById(Guid id)
    {
        var result = await postService.GetPostByIdAsync(id);

        if (!result.Success)
        {
            return NotFound(new { message = result.Message });
        }

        return Ok(result);  // Return the post in the result
    }
    
    [HttpPost("posts/{postId}/like")]
    [Authorize]
    public async Task<IActionResult> LikePost(Guid postId)
    {
        var userId = HttpContext.GetUserId();
        if (userId == null)
            return Unauthorized(new { message = "User not authenticated." });

        var result = await postService.LikePostAsync(userId.Value, postId);
        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }
    
    [HttpDelete("posts/{postId}/like")]
    [Authorize]
    public async Task<IActionResult> UnlikePost(Guid postId)
    {
        var userId = HttpContext.GetUserId();
        if (userId == null)
            return Unauthorized();

        var result = await postService.UnlikePostAsync(userId.Value, postId);

        if (!result.Success)
            return BadRequest(new { message = result.Message });

        return Ok(result);
    }
}