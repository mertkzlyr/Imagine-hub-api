using ImagineHubAPI.Extensions;
using ImagineHubAPI.Interfaces;
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
    public async Task<IActionResult> GetFollowers()
    {
        var userId = HttpContext.GetUserId();
        if (userId == null)
        {
            return Unauthorized(new { message = "User not authenticated." });
        }
        
        var followers = await followService.GetFollowersAsync(userId.Value);
        return Ok(followers);
    }

    [HttpGet("following")]
    public async Task<IActionResult> GetFollowing()
    {
        var userId = HttpContext.GetUserId();
        if (userId == null)
        {
            return Unauthorized(new { message = "User not authenticated." });
        }
        
        var following = await followService.GetFollowingAsync(userId.Value);
        return Ok(following);
    }
}