using ImagineHubAPI.Data;
using ImagineHubAPI.DTOs.CommentDTOs;
using ImagineHubAPI.Interfaces;
using ImagineHubAPI.Models;

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
}