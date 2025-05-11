using ImagineHubAPI.DTOs.CommentDTOs;
using ImagineHubAPI.Models;

namespace ImagineHubAPI.Interfaces;

public interface ICommentRepository
{
    Task<PostComment> CreateAsync(CreateCommentDto commentDto, int userId);
}