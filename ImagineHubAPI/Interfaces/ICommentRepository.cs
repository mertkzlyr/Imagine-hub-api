using ImagineHubAPI.DTOs.CommentDTOs;
using ImagineHubAPI.Models;

namespace ImagineHubAPI.Interfaces;

public interface ICommentRepository
{
    Task<PostComment> CreateAsync(CreateCommentDto commentDto, int userId);
    Task<PostComment> DeleteAsync(Guid commentId, int userId);
    Task<PostComment> UpdateAsync(Guid commentId, int userId, string comment);
}