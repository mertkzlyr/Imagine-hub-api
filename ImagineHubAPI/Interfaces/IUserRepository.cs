using ImagineHubAPI.Models;

namespace ImagineHubAPI.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
}