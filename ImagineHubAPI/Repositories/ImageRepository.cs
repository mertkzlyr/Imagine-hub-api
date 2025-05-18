using ImagineHubAPI.Data;
using ImagineHubAPI.Interfaces;
using ImagineHubAPI.Models;

namespace ImagineHubAPI.Repositories;

public class ImageRepository(DataContext context) : IImageRepository
{
    public async Task SaveImageAsync(Image image)
    {
        await context.Images.AddAsync(image);
        await context.SaveChangesAsync();
    }
}