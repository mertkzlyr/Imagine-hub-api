using ImagineHubAPI.Models;

namespace ImagineHubAPI.Interfaces;

public interface IAdminRepository
{
    Task<List<User>> GetAllUsersAsync();
    Task<Admin> GetByUsernameAsync(string username);
}