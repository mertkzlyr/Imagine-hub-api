using ImagineHubAPI.Models;

namespace ImagineHubAPI.Interfaces;

public interface IImageRepository
{
    Task SaveImageAsync(Models.Image image);
    Task<Models.Image> GetByIdAsync(Guid id, int userId);
    Task<List<Models.Image>> GetByUserIdAsync(int userId, int page, int pageSize);
    Task<int> GetImageCountByUserIdAsync(int userId);
}