using ImagineHubAPI.DTOs.CommentDTOs;
using ImagineHubAPI.Extensions;
using ImagineHubAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImagineHubAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommentController(ICommentService commentService) : ControllerBase
{
    [HttpPost("comment")]
    [Authorize]
    public async Task<IActionResult> CreateComment([FromBody] CreateCommentDto dto)
    {
        var userId = HttpContext.GetUserId();
        if (userId == null)
            return Unauthorized(new { message = "User not authenticated." });

        var result = await commentService.CreateCommentAsync(dto, userId.Value);

        if (!result.Success)
            return BadRequest(new { message = result.Message });

        return Ok(result);
    }

    [HttpDelete("{commentId}")]
    [Authorize]
    public async Task<IActionResult> DeleteComment(Guid commentId)
    {
        var userId = HttpContext.GetUserId();
        if (userId == null)
            return Unauthorized(new { message = "User not authenticated." });

        var result = await commentService.DeleteCommentAsync(commentId, userId.Value);

        if (!result.Success)
            return BadRequest(new { message = result.Message });

        return Ok(result);
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> UpdateComment([FromBody] UpdateCommentDto request)
    {
        var userId = HttpContext.GetUserId();
        if (userId == null)
            return Unauthorized(new { message = "User not authenticated." });
        
        var result = await commentService.UpdateCommentAsync(request.CommentId, userId.Value, request.Comment);

        if (!result.Success)
            return BadRequest(new { message = result.Message });

        return Ok(result);
    }
}