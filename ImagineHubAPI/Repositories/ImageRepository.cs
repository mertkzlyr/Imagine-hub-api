using ImagineHubAPI.Data;
using ImagineHubAPI.Interfaces;
using ImagineHubAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ImagineHubAPI.Repositories;

public class ImageRepository(DataContext context) : IImageRepository
{
    public async Task SaveImageAsync(Image image)
    {
        await context.Images.AddAsync(image);
        await context.SaveChangesAsync();
    }

    public async Task<Image> GetByIdAsync(Guid id, int userId)
    {
        //  with user id
        return await context.Images
            .Where(i => i.Id == id && i.UserId == userId)
            .FirstOrDefaultAsync();
    }

    public async Task<List<Image>> GetByUserIdAsync(int userId, int page, int pageSize)
    {
        //  with user id
        return await context.Images
            .Where(i => i.UserId == userId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
}