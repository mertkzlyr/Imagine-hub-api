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

    public async Task<Result> DeleteCommentAsync(Guid commentId, int userId)
    {
        try
        {
            var deletedComment = await commentRepository.DeleteAsync(commentId, userId);
            if (deletedComment == null)
            {
                return new Result
                {
                    Success = false,
                    Message = "Comment not found or you do not have permission to delete it."
                };
            }

            return new Result
            {
                Success = true,
                Message = "Comment deleted successfully."
            };
        }
        catch (Exception ex)
        {
            return new Result
            {
                Success = false,
                Message = $"Failed to delete comment: {ex.Message}",
            };
        }
    }

    public async Task<Result> UpdateCommentAsync(Guid commentId, int userId, string comment)
    {
        try
        {
            var updatedComment = await commentRepository.UpdateAsync(commentId, userId, comment);
            if (updatedComment == null)
            {
                return new Result
                {
                    Success = false,
                    Message = "Comment not found or you do not have permission to update it."
                };
            }

            return new Result
            {
                Success = true,
                Message = "Comment updated successfully."
            };
        }
        catch (Exception ex)
        {
            return new Result
            {
                Success = false,
                Message = $"Failed to update comment: {ex.Message}",
            };
        }
    }

    public async Task<Result> LikeCommentAsync(Guid commentId, int userId)
    {
        var success = await commentRepository.LikeCommentAsync(userId, commentId);
        return new Result
        {
            Success = success,
            Message = success ? "Comment liked." : "Already liked."
        };
    }

    public async Task<Result> UnlikeCommentAsync(Guid commentId, int userId)
    {
        var success = await commentRepository.UnlikeCommentAsync(userId, commentId);
        return new Result
        {
            Success = success,
            Message = success ? "Comment unliked." : "You haven't liked this comment."
        };
    }
}