using ImagineHubAPI.Models;

namespace ImagineHubAPI.Interfaces;

public interface IImageRepository
{
    Task SaveImageAsync(Image image);
    Task<Image> GetByIdAsync(Guid id, int userId);
    Task<List<Image>> GetByUserIdAsync(int userId, int page, int pageSize);
}