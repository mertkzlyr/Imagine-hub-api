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
}