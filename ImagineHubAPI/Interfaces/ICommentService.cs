using ImagineHubAPI.DTOs.CommentDTOs;
using ImagineHubAPI.Models;

namespace ImagineHubAPI.Interfaces;

public interface ICommentService
{
    Task<Result<CommentDto>> CreateCommentAsync(CreateCommentDto commentDto, int userId);
    Task<Result> DeleteCommentAsync(Guid commentId, int userId);
    Task<Result> UpdateCommentAsync(Guid commentId, int userId, string comment);
    Task<Result> LikeCommentAsync(Guid commentId, int userId);
    Task<Result> UnlikeCommentAsync(Guid commentId, int userId);
}