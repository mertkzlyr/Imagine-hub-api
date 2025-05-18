using ImagineHubAPI.Models;

namespace ImagineHubAPI.Interfaces;

public interface IImageRepository
{
    Task SaveImageAsync(Image image);
}