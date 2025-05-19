using ImagineHubAPI.Models;

namespace ImagineHubAPI.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> RemoveToken(int userId, int token);
}