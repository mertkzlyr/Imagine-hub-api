using ImagineHubAPI.DTOs.CommentDTOs;
using ImagineHubAPI.Models;

namespace ImagineHubAPI.Interfaces;

public interface ICommentService
{
    Task<Result<CommentDto>> CreateCommentAsync(CreateCommentDto commentDto, int userId);
}