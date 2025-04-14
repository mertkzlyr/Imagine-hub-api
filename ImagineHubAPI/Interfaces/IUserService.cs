using ImagineHubAPI.Models;

namespace ImagineHubAPI.Interfaces;

public interface IUserService
{
    Task<User> GetUserByIdAsync(int id);
}