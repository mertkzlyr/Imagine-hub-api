using ImagineHubAPI.Extensions;
using ImagineHubAPI.Interfaces;
using ImagineHubAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImagineHubAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FollowController(IFollowService followService) : ControllerBase
{
    [Authorize]
    [HttpPost("{followeeId}")]
    public async Task<IActionResult> Follow(int followeeId)
    {
        var followerId = HttpContext.GetUserId();
        if (followerId == null)
        {
            return Unauthorized(new { message = "User not authenticated." });
        }
            
        await followService.FollowAsync(followerId.Value, followeeId);
        return Ok(new { message = "Followed successfully." });
    }

    [Authorize]
    [HttpDelete("{followeeId}")]
    public async Task<IActionResult> Unfollow(int followeeId)
    {
        var followerId = HttpContext.GetUserId();
        if (followerId == null)
        {
            return Unauthorized(new { message = "User not authenticated." });
        }
        
        await followService.UnfollowAsync(followerId.Value, followeeId);
        return Ok(new { message = "Unfollowed successfully." });
    }

    [HttpGet("followers")]
    public async Task<IActionResult> GetFollowers([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var userId = HttpContext.GetUserId();
        if (userId == null)
        {
            return Unauthorized(new Result
            {
                Success = false,
                Message = "User not authenticated."
            });
        }

        var followersResult = await followService.GetFollowersAsync(userId.Value, page, pageSize);
        return Ok(followersResult);
    }

    [HttpGet("following")]
    public async Task<IActionResult> GetFollowing([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var userId = HttpContext.GetUserId();
        if (userId == null)
        {
            return Unauthorized(new Result
            {
                Success = false,
                Message = "User not authenticated."
            });
        }

        var followingResult = await followService.GetFollowingAsync(userId.Value, page, pageSize);
        return Ok(followingResult);
    }

}