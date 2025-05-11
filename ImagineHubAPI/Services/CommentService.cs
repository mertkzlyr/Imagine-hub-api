using ImagineHubAPI.DTOs.CommentDTOs;
using ImagineHubAPI.Interfaces;
using ImagineHubAPI.Models;

namespace ImagineHubAPI.Services;

public class CommentService(ICommentRepository commentRepository) : ICommentService 
{
    public async Task<Result<CommentDto>> CreateCommentAsync(CreateCommentDto commentDto, int userId)
    {
        try
        {
            var createdComment = await commentRepository.CreateAsync(commentDto, userId);
            var comment = new CommentDto
            {
                Id = createdComment.Id,
                UserId = createdComment.UserId,
                Comment = createdComment.Comment,
                CreatedAt = createdComment.CreatedAt
            };

            return new Result<CommentDto>
            {
                Success = true,
                Message = "Comment created successfully",
                Data = comment
            };
        }
        catch (Exception ex)
        {
            return new Result<CommentDto>
            {
                Success = false,
                Message = $"Failed to create comment: {ex.Message}",
                Data = null
            };
        }
    }
}