using ImagineHubAPI.Data;
using ImagineHubAPI.Interfaces;
using AppImage = ImagineHubAPI.Models.Image;
using Microsoft.EntityFrameworkCore;

namespace ImagineHubAPI.Repositories;

public class ImageRepository(DataContext context) : IImageRepository
{
    public async Task SaveImageAsync(AppImage image)
    {
        await context.Images.AddAsync(image); // fully qualified to avoid conflict
        await context.SaveChangesAsync();
    }

    public async Task<Models.Image> GetByIdAsync(Guid id, int userId)
    {
        return await context.Images
            .Where((Models.Image i) => i.Id == id && i.UserId == userId) // force the lambda param to be model type
            .FirstOrDefaultAsync();
    }

    public async Task<List<Models.Image>> GetByUserIdAsync(int userId, int page, int pageSize)
    {
        return await context.Images
            .Where((Models.Image i) => i.UserId == userId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetImageCountByUserIdAsync(int userId)
    {
        return await context.Images.CountAsync(i => i.UserId == userId);
    }
}