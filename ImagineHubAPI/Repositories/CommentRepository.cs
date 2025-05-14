using ImagineHubAPI.Data;
using ImagineHubAPI.DTOs.CommentDTOs;
using ImagineHubAPI.Interfaces;
using ImagineHubAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ImagineHubAPI.Repositories;

public class CommentRepository(DataContext context) : ICommentRepository
{
    public async Task<PostComment> CreateAsync(CreateCommentDto commentDto, int userId)
    {
        var comment = new PostComment
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            PostId = commentDto.PostId,
            Comment = commentDto.Comment,
            ParentId = commentDto.ParentId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await context.PostComments.AddAsync(comment);
        await context.SaveChangesAsync();

        return comment;
    }

    public async Task<PostComment> DeleteAsync(Guid commentId, int userId)
    {
        var comment = await context.PostComments
            .FirstOrDefaultAsync(c => c.Id == commentId && c.UserId == userId);

        if (comment != null)
        {
            context.PostComments.Remove(comment);
            await context.SaveChangesAsync();
        }

        return comment;
    }

    public async Task<PostComment> UpdateAsync(Guid commentId, int userId, string comment)
    {
        
        var existingComment = await context.PostComments
            .FirstOrDefaultAsync(c => c.Id == commentId && c.UserId == userId);

        if (existingComment != null)
        {
            existingComment.Comment = comment;
            existingComment.UpdatedAt = DateTime.UtcNow;

            context.PostComments.Update(existingComment);
            await context.SaveChangesAsync();
        }

        return existingComment;
    }

    public async Task<bool> LikeCommentAsync(int userId, Guid commentId)
    {
        var exists = await context.CommentLikes.AnyAsync(cl => cl.UserId == userId && cl.CommentId == commentId);
        if (exists) return false;

        var like = new CommentLike { UserId = userId, CommentId = commentId };
        context.CommentLikes.Add(like);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UnlikeCommentAsync(int userId, Guid commentId)
    {
        var like = await context.CommentLikes
            .FirstOrDefaultAsync(cl => cl.UserId == userId && cl.CommentId == commentId);

        if (like == null) return false;

        context.CommentLikes.Remove(like);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> IsCommentLikedAsync(int userId, Guid commentId)
    {
        return await context.CommentLikes.AnyAsync(cl => cl.UserId == userId && cl.CommentId == commentId);
    }
}