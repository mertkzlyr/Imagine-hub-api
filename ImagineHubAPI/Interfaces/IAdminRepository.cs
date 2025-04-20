using ImagineHubAPI.Models;

namespace ImagineHubAPI.Interfaces;

public interface IAdminRepository : IRepository<Admin>
{
    Task<List<User>> GetAllUsersAsync();
    Task<Admin> GetByUsernameAsync(string username);
}